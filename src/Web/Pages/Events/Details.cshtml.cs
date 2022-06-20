using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Web.Mapping;
using Web.ViewModels;

namespace Web.Pages.Events
{
    public class DetailsModel : PageModel
    {
        private readonly IEventService _eventService;
        private readonly ISaveService _saveService;
        private readonly ILaunchService _launchService;

        public EventDetailsCardViewModel Event { get; set; }

        public DetailsModel(IEventService eventService, ISaveService saveService, ILaunchService launchService)
        {
            _eventService = eventService;
            _saveService = saveService;
            _launchService = launchService;
        }

        public async Task OnGet(int id)
        {
            var result = await _eventService.GetEventAsync(id);
            Event = result.ToEventDetailsCardViewModel();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Event.IsSaved = userId != null && _saveService.IsEventSaved(userId, id);
            if(Event.Launch != null)
            {
                Event.Launch.IsSaved = userId != null && _saveService.IsLaunchSaved(userId, Event.Launch.ApiId);
            }
        }

        public async Task<IActionResult> OnPostToggleSaveLaunch(string launchId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (_saveService.IsLaunchSaved(userId, launchId))
            {
                await _saveService.UnsaveLaunchAsync(userId, launchId);
                return Partial("_SaveToggle", false);
            }
            else
            {
                var launch = await _launchService.GetLaunchAsync(launchId);
                await _saveService.SaveLaunchAsync(userId, launch);
                return Partial("_SaveToggle", true);
            }
        }

        public async Task<IActionResult> OnPostToggleSaveEvent(int eventId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (_saveService.IsEventSaved(userId, eventId))
            {
                await _saveService.UnsaveEventAsync(userId, eventId);
                return Partial("_SaveToggle", false);
            }
            else
            {
                var event_ = await _eventService.GetEventAsync(eventId);
                await _saveService.SaveEventAsync(userId, event_);
                return Partial("_SaveToggle", true);
            }
        }
    }
}
