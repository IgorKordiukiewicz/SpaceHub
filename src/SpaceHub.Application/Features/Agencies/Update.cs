using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Api.Responses;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Enums;

namespace SpaceHub.Application.Features.Agencies;

public record UpdateAgenciesCommand() : IRequest<Result>;

internal class UpdateAgenciesHandler : IRequestHandler<UpdateAgenciesCommand, Result>
{
    private readonly DbContext _db;
    private readonly ILaunchApi _api;
    private readonly int MaxAgenciesPerRequest = 50;

    public UpdateAgenciesHandler(DbContext db, ILaunchApi api)
    {
        _db = db;
        _api = api;
    }

    public async Task<Result> Handle(UpdateAgenciesCommand request, CancellationToken cancellationToken)
    {
        var updateService = new DataUpdateService<AgenciesDetailResponse, AgencyDetailResponse, AgencyModel, int>
        {
            GetItemsCountFunc = _api.GetAgenciesCount,
            GetItemsFunc = idx => _api.GetAgencies(MaxAgenciesPerRequest, idx * MaxAgenciesPerRequest),
            UpdateModelFunc = CreateUpdateModel,
            InsertModelFunc = CreateInsertModel,
            ResponseItemIdSelector = responseItem => responseItem.Id,
            ResponseItemsSelector = response => response.Agencies,
            CollectionSelector = db => db.Agencies,
            ExistingIds = _db.Agencies.AsQueryable().Select(x => x.ApiId).ToHashSet(),
            MaxItemsPerRequest = MaxAgenciesPerRequest,
            CollectionType = ECollection.Agencies,
            Db = _db
        };

        return await updateService.Handle();
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
