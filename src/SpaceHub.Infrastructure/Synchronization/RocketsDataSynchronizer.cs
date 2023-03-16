using MongoDB.Driver;
using Refit;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Api.Responses;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Enums;

namespace SpaceHub.Infrastructure.Synchronization;

public class RocketsDataSynchronizer : LaunchApiDataSynchronizer<RocketsDetailResponse, RocketConfigDetailResponse, RocketModel, int>
{
    public RocketsDataSynchronizer(DbContext db, ILaunchApi api)
        : base(db, api)
    {
    }

    protected override ECollection CollectionType => ECollection.Rockets;

    protected override int MaxItemsPerRequest => 25;

    protected override HashSet<int> CreateExistingIdsHashSet()
        => _db.Rockets.AsQueryable().Select(x => x.ApiId).ToHashSet();

    protected override int GetResponseItemId(RocketConfigDetailResponse item)
        => item.Id;

    protected override Task<IApiResponse<RocketsDetailResponse>> GetItems(int index)
        => _api.GetRockets(MaxItemsPerRequest, index * MaxItemsPerRequest);

    protected override Task<IApiResponse<MultiElementResponse>> GetItemsCount()
        => _api.GetRocketsCount();

    protected override IReadOnlyList<RocketConfigDetailResponse> SelectResponseItems(RocketsDetailResponse response)
        => response.Rockets;

    protected override IMongoCollection<RocketModel> GetCollection(DbContext db)
        => db.Rockets;

    protected override UpdateOneModel<RocketModel> CreateUpdateModel(RocketConfigDetailResponse response)
    {
        var filter = Builders<RocketModel>.Filter.Eq(x => x.ApiId, response.Id);
        var update = Builders<RocketModel>.Update
            .Set(x => x.Active, response.Active)
            .Set(x => x.SuccessfulLaunches, response.SuccessfulLaunches)
            .Set(x => x.TotalLaunches, response.TotalLaunchCount)
            .Set(x => x.ConsecutiveSuccessfulLaunches, response.ConsecutiveSuccessfulLaunches)
            .Set(x => x.PendingLaunches, response.PendingLaunches);
        return new UpdateOneModel<RocketModel>(filter, update);
    }

    protected override InsertOneModel<RocketModel> CreateInsertModel(RocketConfigDetailResponse response)
    {
        long? launchCost = long.TryParse(response.LaunchCost, out var val) ? val : null;

        return new InsertOneModel<RocketModel>(new RocketModel
        {
            ApiId = response.Id,
            Name = response.Name,
            Family = response.Family,
            Variant = response.Variant,
            Active = response.Active,
            Reusable = response.Reusable,
            Description = response.Description,
            ImageUrl = response.ImageUrl ?? string.Empty,
            WikiUrl = response.WikiUrl ?? string.Empty,
            InfoUrl = response.InfoUrl ?? string.Empty,
            Length = response.Length,
            Diameter = response.Diameter,
            MaxStages = response.MaxStage,
            LaunchCost = launchCost,
            LiftoffMass = response.LaunchMass,
            ThrustAtLiftoff = response.ThrustAtLiftoff,
            LeoCapacity = response.LeoCapacity,
            GeoCapacity = response.GeoCapacity,
            SuccessfulLaunches = response.SuccessfulLaunches,
            TotalLaunches = response.TotalLaunchCount,
            ConsecutiveSuccessfulLaunches = response.ConsecutiveSuccessfulLaunches,
            PendingLaunches = response.PendingLaunches,
            FirstFlight = response.FirstFlight,
        });
    }
}
