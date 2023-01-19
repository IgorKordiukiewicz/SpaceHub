using MediatR;
using MongoDB.Driver;
using SpaceHub.Infrastructure.Api.Responses;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Enums;
using SpaceHub.Infrastructure.Data;
using MongoDB.Driver.Linq;
using System.Globalization;
using SpaceHub.Application.Common;

namespace SpaceHub.Application.Features.Launches;

public record UpdateLaunchesCommand() : IRequest;

internal class UpdateLaunchesHandler : IRequestHandler<UpdateLaunchesCommand>
{
    private readonly DbContext _db;
    private readonly ILaunchApi _api;
    private const int MaxLaunchesPerRequest = 25;

    public UpdateLaunchesHandler(DbContext db, ILaunchApi api)
    {
        _db = db;
        _api = api;
    }

    public async Task<Unit> Handle(UpdateLaunchesCommand request, CancellationToken cancellationToken)
    {
        var launches = await GetLaunchesFromApi();
        if(!launches.Any())
        {
            return Unit.Value;
        }

        var existingIds = _db.Launches.AsQueryable().Select(x => x.ApiId).ToHashSet();

        var writes = new List<WriteModel<LaunchModel>>();
        foreach (var launch in launches)
        {
            if (existingIds.Contains(launch.Id))
            {
                writes.Add(CreateUpdateModel(launch));
            }
            else
            {
                writes.Add(CreateInsertModel(launch));
            }
        }

        var _ = await _db.Launches.BulkWriteAsync(writes);

        await _db.CollectionsLastUpdates.UpdateOneAsync(
            x => x.CollectionType == ECollection.Launches,
            Builders<CollectionLastUpdateModel>.Update.Set(x => x.LastUpdate, DateTime.UtcNow));

        return Unit.Value;
    }

    private async Task<List<LaunchDetailResponse>> GetLaunchesFromApi()
    {
        var lastUpdateTime = await _db.CollectionsLastUpdates.AsQueryable()
            .Where(x => x.CollectionType == ECollection.Launches)
            .Select(x => x.LastUpdate)
            .SingleAsync();

        var startDate = lastUpdateTime.ToQueryParameter();
        var endDate = DateTime.UtcNow.ToQueryParameter();
        var count = (await _api.GetLaunchesUpdatedBetweenCountAsync(startDate, endDate))?.Count ?? 0;

        int requestsRequired = ApiHelpers.GetRequiredRequestsCount(count, MaxLaunchesPerRequest);
        var tasks = new List<Task<LaunchesDetailResponse>>();
        for(int i = 0; i < requestsRequired; ++i)
        {
            tasks.Add(_api.GetLaunchesUpdatedBetweenAsync(startDate, endDate, MaxLaunchesPerRequest, i * MaxLaunchesPerRequest));
        }
        await Task.WhenAll(tasks);

        var launches = new List<LaunchDetailResponse>();
        foreach (var task in tasks)
        {
            launches.AddRange(task.Result.Launches);
        }

        return launches;
    }

    private UpdateOneModel<LaunchModel> CreateUpdateModel(LaunchDetailResponse response)
    {
        var filter = Builders<LaunchModel>.Filter.Eq(x => x.ApiId, response.Id);
        var update = Builders<LaunchModel>.Update
            .Set(x => x.Status, response.Status.Name)
            .Set(x => x.Date, response.Date)
            .Set(x => x.WindowStart, response.WindowStart)
            .Set(x => x.WindowEnd, response.WindowEnd);
        return new UpdateOneModel<LaunchModel>(filter, update);
    }

    private InsertOneModel<LaunchModel> CreateInsertModel(LaunchDetailResponse response)
    {
        LaunchMissionModel? mission = null;
        if (response.Mission is not null)
        {
            mission = new()
            {
                Name = response.Mission.Name,
                Type = response.Mission.Type,
                Description = response.Mission.Description,
                OrbitName = response.Mission.Orbit?.Name ?? ""
            };
        }

        var videos = new List<LaunchVideoModel>();
        if (response.Videos is not null)
        {
            foreach (var video in response.Videos)
            {
                videos.Add(new()
                {
                    Url = video.Url,
                    Title = video.Title,
                });
            }
        }

        return new InsertOneModel<LaunchModel>(new LaunchModel()
        {
            ApiId = response.Id,
            Name = response.Name,
            Status = response.Status.Name,
            Date = response.Date,
            WindowStart = response.WindowStart,
            WindowEnd = response.WindowEnd,
            ImageUrl = response.ImageUrl,
            Mission = mission,
            Pad = new()
            {
                Name = response.Pad.Name,
                LocationName = response.Pad.Location.Name,
                CountryCode = response.Pad.Location.CountryCode,
                Latitude = double.Parse(response.Pad.Latitude, CultureInfo.InvariantCulture),
                Longitude = double.Parse(response.Pad.Longitude, CultureInfo.InvariantCulture),
                WikiUrl = response.Pad.WikiUrl ?? "",
                MapImageUrl = response.Pad.Location.MapImageUrl,
                MapUrl = response.Pad.MapUrl ?? ""
            },
            Videos = videos,
            AgencyName = response.Agency.Name,
            AgencyApiId = response.Agency.Id,
            RocketApiId = response.Rocket.Configuration.Id,
        });
    }
}
