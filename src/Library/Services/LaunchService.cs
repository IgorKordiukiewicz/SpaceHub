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

        public LaunchService(ILaunchApi launchApi)
        {
            _launchApi = launchApi;
        }
        
        public async Task<(int, List<Launch>)> GetUpcomingLaunchesAsync(int pageNumber)
        {
            return await GetLaunchesAsync(pageNumber, true);
        }

        public async Task<(int, List<Launch>)> GetPreviousLaunchesAsync(int pageNumber)
        {
            return await GetLaunchesAsync(pageNumber, false);
        }

        private async Task<(int, List<Launch>)> GetLaunchesAsync(int pageNumber, bool upcoming)
        {
            var offset = _pagination.GetOffset(pageNumber);
            var result = upcoming ? 
                await _launchApi.GetUpcomingLaunchesAsync(_pagination.ItemsPerPage, offset)
                : await _launchApi.GetPreviousLaunchesAsync(_pagination.ItemsPerPage, offset);
            var pagesCount = _pagination.GetPagesCount(result.Count);

            return (pagesCount, result.Launches.Select(l => l.ToModel()).ToList());
        }

        public async Task<Launch> GetLaunchAsync(string id)
        {
            var result = await _launchApi.GetLaunchAsync(id);
            
            return result.ToModel();
        }
    }
}
