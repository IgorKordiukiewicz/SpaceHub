using Refit;
using SpaceHub.Infrastructure.Api.Responses;

namespace SpaceHub.Infrastructure.Api;

public interface ILaunchApi
{
    [Get("/launch?mode=list&last_updated__gte={startDate}&last_updated__lte={endDate}&limit=1")]
    Task<IApiResponse<MultiElementResponse>> GetLaunchesUpdatedBetweenCount(string startDate, string endDate);

    [Get("/launch?mode=detailed&last_updated__gte={startDate}&last_updated__lte={endDate}&limit={limit}&offset={offset}")]
    Task<IApiResponse<LaunchesDetailResponse>> GetLaunchesUpdatedBetween(string startDate, string endDate, int limit, int offset);

    [Get("/config/launcher/?mode=list&limit=1")]
    Task<IApiResponse<MultiElementResponse>> GetRocketsCount();

    [Get("/config/launcher/?mode=detailed&limit={limit}&offset={offset}")]
    Task<IApiResponse<RocketsDetailResponse>> GetRockets(int limit, int offset);

    [Get("/agencies/?limit=1")]
    Task<IApiResponse<MultiElementResponse>> GetAgenciesCount();

    [Get("/agencies/?mode=detailed&limit={limit}&offset={offset}")]
    Task<IApiResponse<AgenciesDetailResponse>> GetAgencies(int limit, int offset);
}
