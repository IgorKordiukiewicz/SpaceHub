using Refit;
using SpaceHub.Infrastructure.Api.Responses;

namespace SpaceHub.Infrastructure.Api;

public interface ILaunchApi
{
    [Get("/launch/{timeFrame}/?search={searchValue}&limit={limit}&offset={offset}")]
    Task<LaunchesResponse> GetLaunchesAsync(string timeFrame, string? searchValue, int limit, int offset = 0);

    [Get("/launch/{id}")]
    Task<LaunchDetailResponse> GetLaunchDetailsAsync(string id);
}
