using Library.Api;
using Library.Api.Responses;
using Library.Utils;
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
        private readonly Pagination _pagination = new() { ItemsPerPage = 50 };

        public RocketService(ILaunchApi launchApi)
        {
            _launchApi = launchApi;
        }

        public async Task<(int, List<RocketConfigResponse>)> GetRocketsAsync(int pageNumber)
        {
            int offset = _pagination.GetOffset(pageNumber);
            var result = await _launchApi.GetRocketsAsync(_pagination.ItemsPerPage, offset);
            var pagesCount = _pagination.GetPagesCount(pageNumber);

            return (pagesCount, result.Rockets.ToList());
        }

        public async Task<RocketConfigDetailResponse> GetRocketAsync(int id)
        {
            return await _launchApi.GetRocketAsync(id);
        }
    }
}
