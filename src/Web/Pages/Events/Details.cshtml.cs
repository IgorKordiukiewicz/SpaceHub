using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Mapping;
using Web.ViewModels;

namespace Web.Pages.Events
{
    public class DetailsModel : PageModel
    {
        private readonly IEventService _eventService;

        public EventDetailsViewModel Event { get; set; }

        public DetailsModel(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task OnGet(int id)
        {
            var result = await _eventService.GetEventAsync(id);
            Event = result.ToEventDetailsViewModel();
        }
    }
}
