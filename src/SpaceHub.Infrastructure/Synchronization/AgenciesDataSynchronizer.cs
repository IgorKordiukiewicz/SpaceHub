using MongoDB.Driver;
using Refit;
using SpaceHub.Domain.Models;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Api.Responses;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Enums;

namespace SpaceHub.Infrastructure.Synchronization;

public class AgenciesDataSynchronizer : LaunchApiDataSynchronizer<AgenciesDetailResponse, AgencyDetailResponse, Agency, int>
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

    protected override IMongoCollection<Agency> GetCollection(DbContext db)
        => db.Agencies;

    protected override UpdateOneModel<Agency> CreateUpdateModel(AgencyDetailResponse response)
    {
        var filter = Builders<Agency>.Filter.Eq(x => x.ApiId, response.Id);
        var update = Builders<Agency>.Update
            .Set(x => x.Administrator, response.Administrator)
            .Set(x => x.TotalLaunches, response.TotalLaunchCount)
            .Set(x => x.SuccessfulLaunches, response.SuccessfulLaunches);
        return new UpdateOneModel<Agency>(filter, update);
    }

    protected override InsertOneModel<Agency> CreateInsertModel(AgencyDetailResponse response)
    {
        int? foundingYear = int.TryParse(response.FoundingYear, out var val) ? val : null;

        return new InsertOneModel<Agency>(new Agency
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
