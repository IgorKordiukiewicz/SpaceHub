using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SpaceHub.Application.Common;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Api.Responses;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Enums;

namespace SpaceHub.Application.Features.Rockets;

public record UpdateRocketsCommand() : IRequest;

internal class UpdateRocketsHandler : IRequestHandler<UpdateRocketsCommand>
{
    private readonly DbContext _db;
    private readonly ILaunchApi _api;
    private readonly int MaxRocketsPerRequest = 25;

    public UpdateRocketsHandler(DbContext db, ILaunchApi api)
    {
        _db = db;
        _api = api;
    }

    public async Task<Unit> Handle(UpdateRocketsCommand request, CancellationToken cancellationToken)
    {
        // TODO: Add a check if request was successful, and if not log it
        var rockets = await GetRocketsFromApi();
        if(!rockets.Any())
        {
            return Unit.Value;
        }
        var existingIds = _db.Rockets.AsQueryable().Select(x => x.ApiId).ToHashSet();

        var writes = new List<WriteModel<RocketModel>>();
        foreach(var rocket in rockets)
        {
            if(existingIds.Contains(rocket.Id))
            {
                writes.Add(CreateUpdateModel(rocket));
            }
            else
            {
                writes.Add(CreateInsertModel(rocket));
            }
        }

        var _ = await _db.Rockets.BulkWriteAsync(writes);

        await _db.CollectionsLastUpdates.UpdateOneAsync(
            x => x.CollectionType == ECollection.Rockets,
            Builders<CollectionLastUpdateModel>.Update.Set(x => x.LastUpdate, DateTime.UtcNow));

        return Unit.Value;
    }

    private async Task<List<RocketConfigDetailResponse>> GetRocketsFromApi()
    {
        var count = (await _api.GetRocketsCountAsync())?.Count ?? 0;
        int requestsRequired = ApiHelpers.GetRequiredRequestsCount(count, MaxRocketsPerRequest);

        var tasks = new List<Task<RocketsDetailResponse>>();
        for (int i = 0; i < requestsRequired; ++i)
        {
            tasks.Add(_api.GetRockets(MaxRocketsPerRequest, i * MaxRocketsPerRequest));
        }
        await Task.WhenAll(tasks);

        var rockets = new List<RocketConfigDetailResponse>();
        foreach(var task in tasks)
        {
            rockets.AddRange(task.Result.Rockets);
        }

        return rockets;
    }

    private UpdateOneModel<RocketModel> CreateUpdateModel(RocketConfigDetailResponse rocket)
    {
        var filter = Builders<RocketModel>.Filter.Eq(x => x.ApiId, rocket.Id);
        var update = Builders<RocketModel>.Update
            .Set(x => x.Active, rocket.Active)
            .Set(x => x.SuccessfulLaunches, rocket.SuccessfulLaunches)
            .Set(x => x.TotalLaunches, rocket.TotalLaunchCount)
            .Set(x => x.ConsecutiveSuccessfulLaunches, rocket.ConsecutiveSuccessfulLaunches)
            .Set(x => x.PendingLaunches, rocket.PendingLaunches);
        return new UpdateOneModel<RocketModel>(filter, update);
    }

    private InsertOneModel<RocketModel> CreateInsertModel(RocketConfigDetailResponse rocket)
    {
        long? launchCost = long.TryParse(rocket.LaunchCost, out var val) ? val : null;

        return new InsertOneModel<RocketModel>(new RocketModel
        {
            ApiId = rocket.Id,
            Name = rocket.Name,
            Family = rocket.Family,
            Variant = rocket.Variant,
            Active = rocket.Active,
            Reusable = rocket.Reusable,
            Description = rocket.Description,
            ImageUrl = rocket.ImageUrl ?? string.Empty,
            WikiUrl = rocket.WikiUrl ?? string.Empty,
            InfoUrl = rocket.InfoUrl ?? string.Empty,
            Length = rocket.Length,
            Diameter = rocket.Diameter,
            MaxStages = rocket.MaxStage,
            LaunchCost = launchCost,
            LiftoffMass = rocket.LaunchMass,
            ThrustAtLiftoff = rocket.ThrustAtLiftoff,
            LeoCapacity = rocket.LeoCapacity,
            GeoCapacity = rocket.GeoCapacity,
            SuccessfulLaunches = rocket.SuccessfulLaunches,
            TotalLaunches = rocket.TotalLaunchCount,
            ConsecutiveSuccessfulLaunches = rocket.ConsecutiveSuccessfulLaunches,
            PendingLaunches = rocket.PendingLaunches,
            FirstFlight = rocket.FirstFlight,
        });
    }
}
