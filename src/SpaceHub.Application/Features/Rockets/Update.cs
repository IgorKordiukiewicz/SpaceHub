using FluentResults;
using MediatR;
using MongoDB.Driver;
using SpaceHub.Application.Common;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Api.Responses;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Enums;

namespace SpaceHub.Application.Features.Rockets;

public record UpdateRocketsCommand() : IRequest<Result>;

internal class UpdateRocketsHandler : IRequestHandler<UpdateRocketsCommand, Result>
{
    private readonly DbContext _db;
    private readonly ILaunchApi _api;
    private readonly int MaxRocketsPerRequest = 25;

    public UpdateRocketsHandler(DbContext db, ILaunchApi api)
    {
        _db = db;
        _api = api;
    }

    public async Task<Result> Handle(UpdateRocketsCommand request, CancellationToken cancellationToken)
    {
        var updateService = new DataUpdateService<RocketsDetailResponse, RocketConfigDetailResponse, RocketModel, int>
        {
            GetItemsCountFunc = _api.GetRocketsCount,
            GetItemsFunc = idx => _api.GetRockets(MaxRocketsPerRequest, idx * MaxRocketsPerRequest),
            UpdateModelFunc = CreateUpdateModel,
            InsertModelFunc = CreateInsertModel,
            ResponseItemIdSelector = responseItem => responseItem.Id,
            ResponseItemsSelector = response => response.Rockets,
            CollectionSelector = db => db.Rockets,
            ExistingIds = _db.Rockets.AsQueryable().Select(x => x.ApiId).ToHashSet(),
            MaxItemsPerRequest = MaxRocketsPerRequest,
            CollectionType = ECollection.Rockets,
            Db = _db
        };

        return await updateService.Handle();
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
