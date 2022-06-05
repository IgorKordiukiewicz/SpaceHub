using Library.Api;
using Library.Models;
using Library.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public class LaunchService : ILaunchService
    {
        private readonly ILaunchApi _launchApi;

        public LaunchService(ILaunchApi launchApi)
        {
            _launchApi = launchApi;
        }
        
        public async Task<List<Launch>> GetUpcomingLaunchesAsync()
        {
            var result = await _launchApi.GetUpcomingLaunchesAsync();

            return result.Launches.Select(l => l.ToModel()).ToList();
        }

        public async Task<List<Launch>> GetPreviousLaunchesAsync()
        {
            var result = await _launchApi.GetPreviousLaunchesAsync();

            return result.Launches.Select(l => l.ToModel()).ToList();
        }

        public async Task<Launch> GetLaunchAsync(string id)
        {
            var result = await _launchApi.GetLaunchAsync(id);
            
            return result.ToModel();
        }
    }
}
