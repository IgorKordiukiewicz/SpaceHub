using MediatR;
using MongoDB.Driver;
using SpaceHub.Application.Common;
using SpaceHub.Infrastructure.Api.Responses;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceHub.Infrastructure.Enums;

namespace SpaceHub.Application.Features.Agencies;

public record UpdateAgenciesCommand() : IRequest;

internal class UpdateAgenciesHandler : IRequestHandler<UpdateAgenciesCommand>
{
    private readonly DbContext _db;
    private readonly ILaunchApi _api;
    private readonly int MaxAgenciesPerRequest = 50;

    public UpdateAgenciesHandler(DbContext db, ILaunchApi api)
    {
        _db = db;
        _api = api;
    }

    public async Task<Unit> Handle(UpdateAgenciesCommand request, CancellationToken cancellationToken)
    {
        // TODO: Add a check if request was successful, and if not log it
        var agencies = await GetAgenciesFromApi();
        if (!agencies.Any())
        {
            return Unit.Value;
        }
        var existingIds = _db.Agencies.AsQueryable().Select(x => x.ApiId).ToHashSet();

        var writes = new List<WriteModel<AgencyModel>>();
        foreach (var agency in agencies)
        {
            if (existingIds.Contains(agency.Id))
            {
                writes.Add(CreateUpdateModel(agency));
            }
            else
            {
                writes.Add(CreateInsertModel(agency));
            }
        }

        _ = await _db.Agencies.BulkWriteAsync(writes);

        await _db.CollectionsLastUpdates.UpdateOneAsync(
            x => x.CollectionType == ECollection.Agencies,
            Builders<CollectionLastUpdateModel>.Update.Set(x => x.LastUpdate, DateTime.UtcNow));

        return Unit.Value;
    }

    private async Task<List<AgencyDetailResponse>> GetAgenciesFromApi()
    {
        var count = (await _api.GetRocketsCountAsync())?.Count ?? 0;
        int requestsRequired = ApiHelpers.GetRequiredRequestsCount(count, MaxAgenciesPerRequest);

        var tasks = new List<Task<AgenciesDetailResponse>>();
        for (int i = 0; i < requestsRequired; ++i)
        {
            tasks.Add(_api.GetAgencies(MaxAgenciesPerRequest, i * MaxAgenciesPerRequest));
        }
        await Task.WhenAll(tasks);

        var agencies = new List<AgencyDetailResponse>();
        foreach (var task in tasks)
        {
            agencies.AddRange(task.Result.Agencies);
        }

        return agencies;
    }

    private UpdateOneModel<AgencyModel> CreateUpdateModel(AgencyDetailResponse agency)
    {
        var filter = Builders<AgencyModel>.Filter.Eq(x => x.ApiId, agency.Id);
        var update = Builders<AgencyModel>.Update
            .Set(x => x.Administrator, agency.Administrator)
            .Set(x => x.TotalLaunches, agency.TotalLaunchCount)
            .Set(x => x.SuccessfulLaunches, agency.SuccessfulLaunches);
        return new UpdateOneModel<AgencyModel>(filter, update);
    }

    private InsertOneModel<AgencyModel> CreateInsertModel(AgencyDetailResponse agency)
    {
        int? foundingYear = int.TryParse(agency.FoundingYear, out var val) ? val : null;

        return new InsertOneModel<AgencyModel>(new AgencyModel
        {
            ApiId = agency.Id,
            Name = agency.Name,
            Type = agency.Type,
            CountryCode = agency.CountryCode,
            Description = agency.Description ?? string.Empty,
            Administrator = agency.Administrator ?? string.Empty,
            FoundingYear = foundingYear,
            TotalLaunches = agency.TotalLaunchCount,
            SuccessfulLaunches = agency.SuccessfulLaunches,
            LogoUrl = agency.LogoUrl ?? string.Empty,
            WikiUrl = agency.WikiUrl ?? string.Empty,
            InfoUrl = agency.InfoUrl ?? string.Empty,
        });
    }
}
