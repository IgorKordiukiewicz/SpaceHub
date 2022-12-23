using Infrastructure.Api.Responses;
using Refit;

namespace Infrastructure.Api;

public interface ILaunchApi
{
    [Get("/launch/{timeFrame}/?search={searchValue}&limit={limit}&offset={offset}")]
    Task<LaunchesResponse> GetLaunchesAsync(string timeFrame, string? searchValue, int limit, int offset = 0);
}
