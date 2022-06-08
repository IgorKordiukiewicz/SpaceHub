using Library.Api;
using Library.Models;
using Library.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Utils;

namespace Library.Services
{
    public class LaunchService : ILaunchService
    {
        private readonly ILaunchApi _launchApi;
        private readonly Pagination _pagination = new() { ItemsPerPage = 12 };
        private readonly IRocketService _rocketService;

        public LaunchService(ILaunchApi launchApi, IRocketService rocketService)
        {
            _launchApi = launchApi;
            _rocketService = rocketService;
        }
        
        public async Task<(int, List<Launch>)> GetUpcomingLaunchesAsync(string? searchValue, int pageNumber)
        {
            return await GetLaunchesAsync(searchValue, pageNumber, true);
        }

        public async Task<(int, List<Launch>)> GetPreviousLaunchesAsync(string? searchValue, int pageNumber)
        {
            return await GetLaunchesAsync(searchValue, pageNumber, false);
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
            var result = await _launchApi.GetLaunchAsync(id);

            var launch = result.ToModel();
            await _rocketService.SetRocketRankedProperties(launch.Rocket);
            
            return launch;
        }
    }
}
