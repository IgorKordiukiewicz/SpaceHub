using Library.Enums;
using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Mapping;
using Web.ViewModels;

namespace Web.Pages.Events;

public class IndexModel : PageModel
{
    private readonly IEventService _eventService;
    
    public List<EventIndexCardViewModel> Events { get; set; }
    
    public PaginationViewModel Pagination { get; set; }

    [BindProperty]
    public string? SearchValue { get; set; }

    [BindProperty]
    public DateType EventDateType { get; set; }

    public IndexModel(IEventService eventService)
    {
        _eventService = eventService;
    }
    
    public async Task OnGet(DateType eventDateType, string? searchValue, int pageNumber = 1)
    {
        EventDateType = eventDateType;
        SearchValue = searchValue;

        var (pagesCount, result) = EventDateType == DateType.Upcoming ?
            await _eventService.GetUpcomingEventsAsync(searchValue, pageNumber)
            : await _eventService.GetPreviousEventsAsync(searchValue, pageNumber);

        Events = result?.Select(e => e.ToEventIndexCardViewModel()).ToList();

        Dictionary<string, string> paginationParameters = new()
        {
            { "eventDateType", EventDateType.ToString() }
        };
        if(searchValue != null)
        {
            paginationParameters.Add("searchValue", SearchValue);
        }
        Pagination = new PaginationViewModel(pageNumber, pagesCount, "/Events/Index", paginationParameters);
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("Index", new { eventDateType = EventDateType, searchValue = SearchValue, pageNumber = 1 });
    }
}
