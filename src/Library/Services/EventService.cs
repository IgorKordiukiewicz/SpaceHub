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
            var (pagesCount, result) = await Helpers.GetApiResponseWithSearchAndPagination("upcomingEvents", _launchApi.GetUpcomingEventsAsync, 
                searchValue, pageNumber, _pagination, _cache);

            return (pagesCount, result.Events.Select(e => e.ToModel()).ToList());
        }

        public async Task<(int, List<Event>)> GetPreviousEventsAsync(string? searchValue, int pageNumber)
        {
            var (pagesCount, result) = await Helpers.GetApiResponseWithSearchAndPagination("previousEvents", _launchApi.GetPreviousEventsAsync, 
                searchValue, pageNumber, _pagination, _cache);

            return (pagesCount, result.Events.Select(e => e.ToModel()).ToList());
        }

        public async Task<Event> GetEventAsync(int id)
        {
            var result = await _cache.GetOrCreate("event" + id.ToString(), async entry =>
            {
                return await _launchApi.GetEventAsync(id);
            });

            return result.ToModel();
        }
    }
}
