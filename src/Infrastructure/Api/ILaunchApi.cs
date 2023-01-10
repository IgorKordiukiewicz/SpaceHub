using Refit;
using SpaceHub.Infrastructure.Api.Responses;

namespace SpaceHub.Infrastructure.Api;

public interface ILaunchApi
{
    [Get("/launnch?mode=detailed&last_updated__gte={startDate}&last_updated__lte={endDate}&limit=100&offset={offset}")]
    Task<LaunchesDetailResponse> GetLaunchesUpdatedBetweenAsync(string startDate, string endDate, int offset);
}
