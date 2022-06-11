using Library.Api;
using Library.Mapping;
using Library.Models;
using Library.Utils;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public class EventService : IEventService
    {
        private readonly ILaunchApi _launchApi;
        private readonly Pagination _pagination = new() { ItemsPerPage = 12 };
        private readonly IMemoryCache _cache;

        public EventService(ILaunchApi launchApi, IMemoryCache cache)
        {
            _launchApi = launchApi;
            _cache = cache;
        }

        public async Task<(int, List<Event>)> GetUpcomingEventsAsync(string? searchValue, int pageNumber = 1)
        {
            var offset = _pagination.GetOffset(pageNumber);
            var result = await _cache.GetOrCreateAsync("events" + searchValue + pageNumber.ToString(), async entry =>
            {
                return await _launchApi.GetUpcomingEventsAsync(searchValue, _pagination.ItemsPerPage, offset);
            });
            var pagesCount = _pagination.GetPagesCount(result.Count);

            return (pagesCount, result.Events.Select(e => e.ToModel()).ToList());
        }
    }
}
