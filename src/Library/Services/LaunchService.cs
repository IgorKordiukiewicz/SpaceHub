using Library.Api;
using Library.Models;
using Library.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Utils;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;
using Library.Api.Responses;

namespace Library.Services
{
    public class LaunchService : ILaunchService
    {
        private readonly ILaunchApi _launchApi;
        private readonly IRocketService _rocketService;
        private readonly IMemoryCache _cache;

        public Pagination Pagination { get; } = new() { ItemsPerPage = 12 };

        public LaunchService(ILaunchApi launchApi, IRocketService rocketService, IMemoryCache cache)
        {
            _launchApi = launchApi;
            _rocketService = rocketService;
            _cache = cache;
        }
        
        public async Task<(int, List<Launch>)> GetUpcomingLaunchesAsync(string? searchValue, int pageNumber)
        {
            var (pagesCount, result) = await Helpers.GetApiResponseWithSearchAndPagination("upcomingLaunches", _launchApi.GetUpcomingLaunchesAsync, 
                searchValue, pageNumber, Pagination, _cache);

            return (pagesCount, result.Launches.Select(l => l.ToModel()).ToList());
        }

        public async Task<(int, List<Launch>)> GetPreviousLaunchesAsync(string? searchValue, int pageNumber)
        {
            var (pagesCount, result) = await Helpers.GetApiResponseWithSearchAndPagination("previousLaunches", _launchApi.GetPreviousLaunchesAsync, 
                searchValue, pageNumber, Pagination, _cache);

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
}
