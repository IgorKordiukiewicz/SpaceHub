using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SpaceHub.Domain.Enums;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Api.Responses;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Enums;
using System.Globalization;
using System.Xml.Linq;

namespace SpaceHub.Infrastructure.Services;

// should it be here or moved to Application/Features
// maybe the background service should call mediatr which has handlers for updateArticles, Launches, etc
public class DataUpdateService : BackgroundService
{
    private readonly IArticleApi _articleApi;
    private readonly DbContext _db;
    private readonly PeriodicTimer _timer;
    private readonly ILaunchApi _launchApi;

    public DataUpdateService(IArticleApi articleApi, DbContext db, IOptions<InfrastructureSettings> settingsOptions, ILaunchApi launchApi)
    {
        _articleApi = articleApi;
        _db = db;

        _timer = new PeriodicTimer(TimeSpan.FromMinutes(settingsOptions.Value.DataUpdateEveryXMinutes));
        _launchApi = launchApi;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(await _timer.WaitForNextTickAsync(stoppingToken)
            && !stoppingToken.IsCancellationRequested)
        {
            await UpdateArticles();
            await UpdateLaunches();
        }
    }

    private async Task UpdateArticles()
    {
        var lastUpdateTime = await _db.CollectionsLastUpdates.AsQueryable()
            .Where(x => x.CollectionType == ECollection.Articles)
            .Select(x => x.LastUpdate)
            .SingleAsync();

        var now = DateTime.UtcNow; // TODO: Use some kind of interface for DateTime
        var articles = await _articleApi.GetArticlesPublishedBetweenAsync(lastUpdateTime.ToQueryParameter(), now.ToQueryParameter());
        if(articles is null)
        {
            // TODO: log,
            // method should return Result<T>, if this method's result is bad then maybe add background job in hangfire to retry in e.g. 30minutes
            return;
        }

        if(articles.Any())
        {
            var newArticles = new List<ArticleModel>();
            foreach (var article in articles)
            {
                newArticles.Add(new ArticleModel()
                {
                    Title = article.Title,
                    Summary = article.Summary,
                    ImageUrl = article.ImageUrl,
                    NewsSite = article.NewsSite,
                    PublishDate = article.PublishDate,
                    Url = article.Url,
                });
            }

            await _db.Articles.InsertManyAsync(newArticles);
        }

        await _db.CollectionsLastUpdates.UpdateOneAsync(
            x => x.CollectionType == ECollection.Articles,
            Builders<CollectionLastUpdateModel>.Update.Set(x => x.LastUpdate, now));
    }

    private async Task UpdateLaunches()
    {
        var lastUpdateTime = await _db.CollectionsLastUpdates.AsQueryable()
            .Where(x => x.CollectionType == ECollection.Launches)
            .Select(x => x.LastUpdate)
            .SingleAsync();

        var now = DateTime.UtcNow;
        var launches = new List<LaunchDetailResponse>();
        int offset = 0;
        while(true)
        {
            // TODO: Inefficient, maybe break when Launches.Count < 100 (limit) to avoid having to call api one more time
            var result = await _launchApi.GetLaunchesUpdatedBetweenAsync(lastUpdateTime.ToQueryParameter(), now.ToQueryParameter(), offset);
            if(!result.Launches.Any())
            {
                break;
            }

            launches.AddRange(result.Launches);
            offset += 100;
        }

        var existingIds = _db.Launches.AsQueryable().Select(x => x.ApiId).ToHashSet();

        var writes = new List<WriteModel<LaunchModel>>();
        foreach(var launch in launches)
        {
            if(existingIds.Contains(launch.Id))
            {
                var filter = Builders<LaunchModel>.Filter.Eq(x => x.ApiId, launch.Id);
                var update = Builders<LaunchModel>.Update
                    .Set(x => x.Status, launch.Status.Name)
                    .Set(x => x.Date, launch.Date)
                    .Set(x => x.WindowStart, launch.WindowStart)
                    .Set(x => x.WindowEnd, launch.WindowEnd);
                writes.Add(new UpdateOneModel<LaunchModel>(filter, update));
            }
            else
            {
                LaunchMissionModel? mission = null;
                if (launch.Mission is not null)
                {
                    mission = new()
                    {
                        Name = launch.Mission.Name,
                        Type = launch.Mission.Type,
                        Description = launch.Mission.Description,
                        OrbitName = launch.Mission.Orbit?.Name ?? ""
                    };
                }

                var videos = new List<LaunchVideoModel>();
                if (launch.Videos is not null)
                {
                    foreach (var video in launch.Videos)
                    {
                        videos.Add(new()
                        {
                            Url = video.Url,
                            Title = video.Title,
                        });
                    }
                }

                var model = new LaunchModel()
                {
                    ApiId = launch.Id,
                    Name = launch.Name,
                    Status = launch.Status.Name,
                    Date = launch.Date,
                    WindowStart = launch.WindowStart,
                    WindowEnd = launch.WindowEnd,
                    ImageUrl = launch.ImageUrl,
                    Mission = mission,
                    Pad = new()
                    {
                        Name = launch.Pad.Name,
                        LocationName = launch.Pad.Location.Name,
                        CountryCode = launch.Pad.Location.CountryCode,
                        Latitude = double.Parse(launch.Pad.Latitude, CultureInfo.InvariantCulture),
                        Longitude = double.Parse(launch.Pad.Longitude, CultureInfo.InvariantCulture),
                        WikiUrl = launch.Pad.WikiUrl ?? "",
                        MapImageUrl = launch.Pad.Location.MapImageUrl,
                        MapUrl = launch.Pad.MapUrl ?? ""
                    },
                    Videos = videos,
                    AgencyName = launch.Agency.Name,
                    AgencyApiId = launch.Agency.Id,
                    RocketApiId = launch.Rocket.Configuration.Id,
                };
                writes.Add(new InsertOneModel<LaunchModel>(model));
            }
        }

        var _ = await _db.Launches.BulkWriteAsync(writes);

        await _db.CollectionsLastUpdates.UpdateOneAsync(
            x => x.CollectionType == ECollection.Launches,
            Builders<CollectionLastUpdateModel>.Update.Set(x => x.LastUpdate, now));
    }
}
