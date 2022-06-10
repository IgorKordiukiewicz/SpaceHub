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

namespace Library.Services
{
    public class LaunchService : ILaunchService
    {
        private readonly ILaunchApi _launchApi;
        private readonly Pagination _pagination = new() { ItemsPerPage = 12 };
        private readonly IRocketService _rocketService;
        private readonly IMemoryCache _cache;

        public LaunchService(ILaunchApi launchApi, IRocketService rocketService, IMemoryCache cache)
        {
            _launchApi = launchApi;
            _rocketService = rocketService;
            _cache = cache;
        }
        
        public async Task<(int, List<Launch>)> GetUpcomingLaunchesAsync(string? searchValue, int pageNumber)
        {
            var result = await _cache.GetOrCreateAsync("upcomingLaunches" + searchValue + pageNumber.ToString(), async entry =>
            {
                return await GetLaunchesAsync(searchValue, pageNumber, true);
            });

            return result;
        }

        public async Task<(int, List<Launch>)> GetPreviousLaunchesAsync(string? searchValue, int pageNumber)
        {
            var result = await _cache.GetOrCreateAsync("upcomingLaunches" + searchValue + pageNumber.ToString(), async entry =>
            {
                return await GetLaunchesAsync(searchValue, pageNumber, false);
            });

            return result;
        }

        private async Task<(int, List<Launch>)> GetLaunchesAsync(string? searchValue, int pageNumber, bool upcoming)
        {
            var offset = _pagination.GetOffset(pageNumber);
            var result = upcoming ? 
                await _launchApi.GetUpcomingLaunchesAsync(searchValue, _pagination.ItemsPerPage, offset)
                : await _launchApi.GetPreviousLaunchesAsync(searchValue, _pagination.ItemsPerPage, offset);
            var pagesCount = _pagination.GetPagesCount(result.Count);

            return (pagesCount, result.Launches.Select(l => l.ToModel()).ToList());
        }

        public async Task<Launch> GetLaunchAsync(string id)
        {
            var result = await _cache.GetOrCreateAsync("launch" + id, async entry =>
            {
                return await _launchApi.GetLaunchAsync(id);
            });

            var launch = result.ToModel();
            await _rocketService.SetRocketRankedProperties(launch.Rocket);
            
            return launch;
        }
    }
}
