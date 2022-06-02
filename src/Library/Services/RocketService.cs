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

        public async Task<(int, List<RocketConfigResponse>)> GetRocketsAsync(int pageNumber)
        {
            int limit = 50;
            int offset = (pageNumber - 1) * limit;
            var result = await _launchApi.GetRocketsAsync(limit, offset);
            var pagesCount = (result.Count - 1) / limit + 1;
            return (pagesCount, result.Rockets.ToList());
        }
    }
}
