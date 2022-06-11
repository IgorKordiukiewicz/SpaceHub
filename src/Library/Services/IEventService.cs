using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public interface IEventService
    {
        Task<(int, List<Event>)> GetUpcomingEventsAsync(string? searchValue, int pageNumber = 1);

        Task<(int, List<Event>)> GetPreviousEventsAsync(string? searchValue, int pageNumber = 1);

        Task<Event> GetEventAsync(int id);
    }
}
