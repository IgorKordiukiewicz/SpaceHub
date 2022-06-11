using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Mapping;
using Web.ViewModels;

namespace Web.Pages.Events
{
    public class IndexModel : PageModel
    {
        private readonly IEventService _eventService;
        
        public List<EventCardViewModel>? Events { get; set; }
        
        public PaginationViewModel Pagination { get; set; }

        [BindProperty]
        public string? SearchValue { get; set; }

        public IndexModel(IEventService eventService)
        {
            _eventService = eventService;
        }
        
        public async Task OnGet(string? searchValue, int pageNumber = 1)
        {
            SearchValue = searchValue;

            var (pagesCount, result) = await _eventService.GetUpcomingEventsAsync(searchValue, pageNumber);

            Events = result?.Select(e => e.ToEventCardViewModel()).ToList();

            Pagination = new PaginationViewModel(pageNumber, pagesCount, "/Events/Index", searchValue != null ? new() { { "searchValue", searchValue } } : null);
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Index", new { searchValue = SearchValue, pageNumber = 1 });
        }
    }
}
