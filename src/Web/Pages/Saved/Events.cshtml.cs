using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Web.Mapping;
using Web.ViewModels;

namespace Web.Pages.Saved;

[Authorize]
public class EventsModel : PageModel
{
    private readonly ISaveService _saveService;

    public List<EventIndexCardViewModel> Events { get; set; }

    public PaginationViewModel Pagination { get; set; }

    public EventsModel(ISaveService saveService)
    {
        _saveService = saveService;
    }

    public void OnGet(int pageNumber = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Events = _saveService.GetSavedEvents(userId, pageNumber).Select(l => l.ToEventIndexCardViewModel()).ToList();

        var pagesCount = _saveService.GetSavedEventsPagesCount(userId);
        Pagination = new(pageNumber, pagesCount, "/Saved/Events");
    }
}
