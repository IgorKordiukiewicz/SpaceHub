using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.ViewModels;

namespace Web.Pages.Rockets
{
    public class IndexModel : PageModel
    {
        private readonly IRocketService _rocketService;

        public List<RocketIndexViewModel>? Rockets { get; set; }

        [BindProperty]
        public int PageNumber { get; set; }

        public int PagesCount { get; set; } = 1;

        public IndexModel(IRocketService rocketService)
        {
            _rocketService = rocketService;
        }

        public async Task OnGet(int pageNumber = 1)
        {
            PageNumber = pageNumber;
            
            var (pagesCount, result) = await _rocketService.GetRocketsAsync(PageNumber);
            PagesCount = pagesCount;
            Rockets = result?.Select(r => new RocketIndexViewModel(r)).ToList();
        }
    }
}
