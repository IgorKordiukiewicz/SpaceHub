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

        public async Task<(int, List<Event>)> GetUpcomingEventsAsync(string? searchValue, int pageNumber)
        {
            var result = await _cache.GetOrCreateAsync("upcomingEvents" + searchValue + pageNumber.ToString(), async entry =>
            {
                return await GetEventsAsync(searchValue, pageNumber, true);
            });

            return result;
        }

        public async Task<(int, List<Event>)> GetPreviousEventsAsync(string? searchValue, int pageNumber)
        {
            var result = await _cache.GetOrCreateAsync("previousEvents" + searchValue + pageNumber.ToString(), async entry =>
            {
                return await GetEventsAsync(searchValue, pageNumber, false);
            });

            return result;
        }

        private async Task<(int, List<Event>)> GetEventsAsync(string? searchValue, int pageNumber, bool upcoming)
        {
            var offset = _pagination.GetOffset(pageNumber);
            var result = upcoming ?
                await _launchApi.GetUpcomingEventsAsync(searchValue, _pagination.ItemsPerPage, offset)
                : await _launchApi.GetPreviousEventsAsync(searchValue, _pagination.ItemsPerPage, offset);
            var pagesCount = _pagination.GetPagesCount(result.Count);

            return (pagesCount, result.Events.Select(e => e.ToModel()).ToList());
        }
    }
}
