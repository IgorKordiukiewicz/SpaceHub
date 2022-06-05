using Library.Api;
using Library.Models;
using Library.Mapping;
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

        public async Task<(int, List<Rocket>)> GetRocketsAsync(int pageNumber)
        {
            int offset = _pagination.GetOffset(pageNumber);
            var result = await _launchApi.GetRocketsAsync(_pagination.ItemsPerPage, offset);
            var pagesCount = _pagination.GetPagesCount(result.Count);

            return (pagesCount, result.Rockets.Select(r => r.ToModel()).ToList());
        }

        public async Task<Rocket> GetRocketAsync(int id)
        {
            var result = await _launchApi.GetRocketAsync(id);

            return result.ToModel();
        }
    }
}
