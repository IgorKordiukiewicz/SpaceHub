using Library.Api;
using Library.Models;
using Library.Mapping;
using Microsoft.Extensions.Caching.Memory;
using Library.Services.Interfaces;

namespace Library.Services;

public class LaunchService : ILaunchService
{
    private readonly ILaunchApi _launchApi;
    private readonly IRocketService _rocketService;
    private readonly IMemoryCache _cache;

    public LaunchService(ILaunchApi launchApi, IRocketService rocketService, IMemoryCache cache)
    {
        _launchApi = launchApi;
        _rocketService = rocketService;
        _cache = cache;
    }
    
    public async Task<(int, List<Launch>)> GetUpcomingLaunchesAsync(string? searchValue, int pageNumber, int itemsPerPage)
    {
        var (pagesCount, result) = await Helpers.GetApiResponseWithSearchAndPagination("upcomingLaunches", _launchApi.GetUpcomingLaunchesAsync, 
            searchValue, pageNumber, itemsPerPage, _cache);

        return (pagesCount, result.Launches.Select(l => l.ToModel()).ToList());
    }

    public async Task<(int, List<Launch>)> GetPreviousLaunchesAsync(string? searchValue, int pageNumber, int itemsPerPage)
    {
        var (pagesCount, result) = await Helpers.GetApiResponseWithSearchAndPagination("previousLaunches", _launchApi.GetPreviousLaunchesAsync, 
            searchValue, pageNumber, itemsPerPage, _cache);

        return (pagesCount, result.Launches.Select(l => l.ToModel()).ToList());
    }

    public async Task<Launch> GetLaunchAsync(string id)
    {
        var result = await _cache.GetOrCreateAsync("launch" + id, async entry =>
        {
            return await _launchApi.GetLaunchAsync(id);
        });

        var launch = result.ToModel();
        await _rocketService.SetRocketRankedPropertiesAsync(launch.Rocket);
        
        return launch;
    }
}
