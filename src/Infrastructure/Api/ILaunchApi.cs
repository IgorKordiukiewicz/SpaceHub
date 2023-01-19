using Refit;
using SpaceHub.Infrastructure.Api.Responses;

namespace SpaceHub.Infrastructure.Api;

public interface ILaunchApi
{
    [Get("/launch?mode=detailed&last_updated__gte={startDate}&last_updated__lte={endDate}&limit={limit}&offset={offset}")]
    Task<LaunchesDetailResponse> GetLaunchesUpdatedBetweenAsync(string startDate, string endDate, int limit, int offset);

    [Get("/launch?mode=list&last_updated__gte={startDate}&last_updated__lte={endDate}&limit=1")]
    Task<MultiElementResponse> GetLaunchesUpdatedBetweenCountAsync(string startDate, string endDate);
}
