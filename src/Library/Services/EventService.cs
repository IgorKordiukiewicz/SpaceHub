using Library.Api;
using Library.Mapping;
using Library.Models;
using Microsoft.Extensions.Caching.Memory;
using Library.Services.Interfaces;

namespace Library.Services;

public class EventService : IEventService
{
    private readonly ILaunchApi _launchApi;
    private readonly IMemoryCache _cache;

    public EventService(ILaunchApi launchApi, IMemoryCache cache)
    {
        _launchApi = launchApi;
        _cache = cache;
    }

    public async Task<(int, List<Event>)> GetUpcomingEventsAsync(string? searchValue, int pageNumber, int itemsPerPage)
    {
        var (pagesCount, result) = await Helpers.GetApiResponseWithSearchAndPagination("upcomingEvents", _launchApi.GetUpcomingEventsAsync, 
            searchValue, pageNumber, itemsPerPage, _cache);

        return (pagesCount, result.Events.Select(e => e.ToModel()).ToList());
    }

    public async Task<(int, List<Event>)> GetPreviousEventsAsync(string? searchValue, int pageNumber, int itemsPerPage)
    {
        var (pagesCount, result) = await Helpers.GetApiResponseWithSearchAndPagination("previousEvents", _launchApi.GetPreviousEventsAsync, 
            searchValue, pageNumber, itemsPerPage, _cache);

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
