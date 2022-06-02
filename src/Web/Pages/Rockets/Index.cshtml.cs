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

        public PaginationViewModel Pagination { get; set; }

        public IndexModel(IRocketService rocketService)
        {
            _rocketService = rocketService;
        }

        public async Task OnGet(int pageNumber = 1)
        {
            var (pagesCount, result) = await _rocketService.GetRocketsAsync(pageNumber);

            Rockets = result?.Select(r => new RocketIndexViewModel(r)).ToList();

            Pagination = new PaginationViewModel(pageNumber, pagesCount, "/Rockets/Index");
        }
    }
}
