using MongoDB.Driver;
using Refit;
using SpaceHub.Domain.Models;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Api.Responses;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Enums;
using System.Globalization;

namespace SpaceHub.Infrastructure.Synchronization;

public class LaunchesDataSynchronizer : LaunchApiDataSynchronizer<LaunchesDetailResponse, LaunchDetailResponse, Launch, Guid>
{
    public LaunchesDataSynchronizer(DbContext db, ILaunchApi api)
        : base(db, api)
    {
    }

    protected override ECollection CollectionType => ECollection.Launches;

    protected override int MaxItemsPerRequest => 25;

    protected override HashSet<Guid> CreateExistingIdsHashSet()
        => _db.Launches.AsQueryable().Select(x => x.ApiId).ToHashSet();

    protected override Guid GetResponseItemId(LaunchDetailResponse item)
        => item.Id;

    protected override Task<IApiResponse<LaunchesDetailResponse>> GetItems(int index)
        => _api.GetLaunchesUpdatedBetween(_startDateParameter, _endDateParameter, MaxItemsPerRequest, index * MaxItemsPerRequest);

    protected override Task<IApiResponse<MultiElementResponse>> GetItemsCount()
        => _api.GetLaunchesUpdatedBetweenCount(_startDateParameter, _endDateParameter);

    protected override IReadOnlyList<LaunchDetailResponse> SelectResponseItems(LaunchesDetailResponse response)
        => response.Launches;

    protected override IMongoCollection<Launch> GetCollection(DbContext db)
        => db.Launches;

    protected override UpdateOneModel<Launch> CreateUpdateModel(LaunchDetailResponse response)
    {
        var filter = Builders<Launch>.Filter.Eq(x => x.ApiId, response.Id);
        var update = Builders<Launch>.Update
            .Set(x => x.Status, response.Status.Name)
            .Set(x => x.Date, response.Date)
            .Set(x => x.WindowStart, response.WindowStart)
            .Set(x => x.WindowEnd, response.WindowEnd);
        return new UpdateOneModel<Launch>(filter, update);
    }

    protected override InsertOneModel<Launch> CreateInsertModel(LaunchDetailResponse response)
    {
        LaunchMission? mission = null;
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

        var videos = new List<LaunchVideo>();
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

        return new InsertOneModel<Launch>(new Launch()
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
