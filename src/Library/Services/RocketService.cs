using Library.Api;
using Library.Api.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public class RocketService : IRocketService
    {
        private readonly ILaunchApi _launchApi;

        public RocketService(ILaunchApi launchApi)
        {
            _launchApi = launchApi;
        }
        
        public async Task<List<RocketConfigResponse>> GetRockets()
        {
            var result = await _launchApi.GetRockets();
            return result.Rockets.ToList();
        }
    }
}
