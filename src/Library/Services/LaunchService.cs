using Library.Api;
using Library.Api.Responses;
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
        
        public async Task<List<LaunchResponse>> GetUpcomingLaunchesAsync()
        {
            var result = await _launchApi.GetUpcomingLaunchesAsync();

            return result.Launches.ToList();
        }

        public async Task<List<LaunchResponse>> GetPreviousLaunchesAsync()
        {
            var result = await _launchApi.GetPreviousLaunchesAsync();

            return result.Launches.ToList();
        }

        public async Task<LaunchDetailResponse> GetLaunchAsync(string id)
        {
            return await _launchApi.GetLaunchAsync(id);
        }
    }
}
