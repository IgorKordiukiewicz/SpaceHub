using FluentResults;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SpaceHub.Application.Common;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Api.Responses;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Enums;
using System.Globalization;

namespace SpaceHub.Application.Features.Launches;

public record UpdateLaunchesCommand() : IRequest<Result>;

internal class UpdateLaunchesHandler : IRequestHandler<UpdateLaunchesCommand, Result>
{
    private readonly DbContext _db;
    private readonly ILaunchApi _api;
    private const int MaxLaunchesPerRequest = 25;

    public UpdateLaunchesHandler(DbContext db, ILaunchApi api)
    {
        _db = db;
        _api = api;
    }

    public async Task<Result> Handle(UpdateLaunchesCommand request, CancellationToken cancellationToken)
    {
        var lastUpdateTime = await _db.CollectionsLastUpdates.AsQueryable()
            .Where(x => x.CollectionType == ECollection.Launches)
            .Select(x => x.LastUpdate)
            .SingleAsync();

        var startDate = lastUpdateTime.ToQueryParameter();
        var endDate = DateTime.UtcNow.ToQueryParameter();

        var updateService = new DataUpdateService<LaunchesDetailResponse, LaunchDetailResponse, LaunchModel, string>
        {
            GetItemsCountFunc = () => _api.GetLaunchesUpdatedBetweenCount(startDate, endDate),
            GetItemsFunc = idx => _api.GetLaunchesUpdatedBetween(startDate, endDate, MaxLaunchesPerRequest, idx * MaxLaunchesPerRequest),
            UpdateModelFunc = CreateUpdateModel,
            InsertModelFunc = CreateInsertModel,
            ResponseItemIdSelector = responseItem => responseItem.Id,
            ResponseItemsSelector = response => response.Launches,
            CollectionSelector = db => db.Launches,
            ExistingIds = _db.Launches.AsQueryable().Select(x => x.ApiId).ToHashSet(),
            MaxItemsPerRequest = MaxLaunchesPerRequest,
            CollectionType = ECollection.Launches,
            Db = _db
        };

        return await updateService.Handle();
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
