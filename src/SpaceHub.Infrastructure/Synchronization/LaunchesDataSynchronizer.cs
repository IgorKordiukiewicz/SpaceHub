using MongoDB.Driver;
using Refit;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Api.Responses;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Enums;
using System.Globalization;

namespace SpaceHub.Infrastructure.Synchronization;

public class LaunchesDataSynchronizer : LaunchApiDataSynchronizer<LaunchesDetailResponse, LaunchDetailResponse, LaunchModel, string>
{
    public LaunchesDataSynchronizer(DbContext db, ILaunchApi api)
        : base(db, api)
    {
    }

    protected override ECollection CollectionType => ECollection.Launches;

    protected override int MaxItemsPerRequest => 25;

    protected override HashSet<string> CreateExistingIdsHashSet()
        => _db.Launches.AsQueryable().Select(x => x.ApiId).ToHashSet();

    protected override string GetResponseItemId(LaunchDetailResponse item)
        => item.Id;

    protected override Task<IApiResponse<LaunchesDetailResponse>> GetItems(int index)
        => _api.GetLaunchesUpdatedBetween(_startDateParameter, _endDateParameter, MaxItemsPerRequest, index * MaxItemsPerRequest);

    protected override Task<IApiResponse<MultiElementResponse>> GetItemsCount()
        => _api.GetLaunchesUpdatedBetweenCount(_startDateParameter, _endDateParameter);

    protected override IReadOnlyList<LaunchDetailResponse> SelectResponseItems(LaunchesDetailResponse response)
        => response.Launches;

    protected override IMongoCollection<LaunchModel> GetCollection(DbContext db)
        => db.Launches;

    protected override UpdateOneModel<LaunchModel> CreateUpdateModel(LaunchDetailResponse response)
    {
        var filter = Builders<LaunchModel>.Filter.Eq(x => x.ApiId, response.Id);
        var update = Builders<LaunchModel>.Update
            .Set(x => x.Status, response.Status.Name)
            .Set(x => x.Date, response.Date)
            .Set(x => x.WindowStart, response.WindowStart)
            .Set(x => x.WindowEnd, response.WindowEnd);
        return new UpdateOneModel<LaunchModel>(filter, update);
    }

    protected override InsertOneModel<LaunchModel> CreateInsertModel(LaunchDetailResponse response)
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
