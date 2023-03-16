using MongoDB.Driver;
using Refit;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Api.Responses;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Enums;

namespace SpaceHub.Infrastructure.Synchronization;

public class AgenciesDataSynchronizer : LaunchApiDataSynchronizer<AgenciesDetailResponse, AgencyDetailResponse, AgencyModel, int>
{
    public AgenciesDataSynchronizer(DbContext db, ILaunchApi api)
        : base(db, api)
    {
    }

    protected override ECollection CollectionType => ECollection.Agencies;

    protected override int MaxItemsPerRequest => 50;

    protected override HashSet<int> CreateExistingIdsHashSet()
        => _db.Agencies.AsQueryable().Select(x => x.ApiId).ToHashSet();

    protected override int GetResponseItemId(AgencyDetailResponse item)
        => item.Id;

    protected override Task<IApiResponse<AgenciesDetailResponse>> GetItems(int index)
        => _api.GetAgencies(MaxItemsPerRequest, index * MaxItemsPerRequest);

    protected override Task<IApiResponse<MultiElementResponse>> GetItemsCount()
        => _api.GetAgenciesCount();

    protected override IReadOnlyList<AgencyDetailResponse> SelectResponseItems(AgenciesDetailResponse response)
        => response.Agencies;

    protected override IMongoCollection<AgencyModel> GetCollection(DbContext db)
        => db.Agencies;

    protected override UpdateOneModel<AgencyModel> CreateUpdateModel(AgencyDetailResponse response)
    {
        var filter = Builders<AgencyModel>.Filter.Eq(x => x.ApiId, response.Id);
        var update = Builders<AgencyModel>.Update
            .Set(x => x.Administrator, response.Administrator)
            .Set(x => x.TotalLaunches, response.TotalLaunchCount)
            .Set(x => x.SuccessfulLaunches, response.SuccessfulLaunches);
        return new UpdateOneModel<AgencyModel>(filter, update);
    }

    protected override InsertOneModel<AgencyModel> CreateInsertModel(AgencyDetailResponse response)
    {
        int? foundingYear = int.TryParse(response.FoundingYear, out var val) ? val : null;

        return new InsertOneModel<AgencyModel>(new AgencyModel
        {
            ApiId = response.Id,
            Name = response.Name,
            Type = response.Type,
            CountryCode = response.CountryCode,
            Description = response.Description ?? string.Empty,
            Administrator = response.Administrator ?? string.Empty,
            FoundingYear = foundingYear,
            TotalLaunches = response.TotalLaunchCount,
            SuccessfulLaunches = response.SuccessfulLaunches,
            LogoUrl = response.LogoUrl ?? string.Empty,
            WikiUrl = response.WikiUrl ?? string.Empty,
            InfoUrl = response.InfoUrl ?? string.Empty,
        });
    }
}
