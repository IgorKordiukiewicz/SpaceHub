using Library.Models;

namespace Library.Services;

public interface IEventService
{ 
    Task<(int, List<Event>)> GetUpcomingEventsAsync(string? searchValue, int pageNumber = 1, int itemsPerPage = 12);

    Task<(int, List<Event>)> GetPreviousEventsAsync(string? searchValue, int pageNumber = 1, int itemsPerPage = 12);

    Task<Event> GetEventAsync(int id);
}
