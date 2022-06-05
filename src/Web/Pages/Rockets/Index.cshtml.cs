using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.ViewModels;
using Web.Mapping;

namespace Web.Pages.Rockets
{
    public class IndexModel : PageModel
    {
        private readonly IRocketService _rocketService;

        public List<RocketListItemViewModel>? Rockets { get; set; }

        public PaginationViewModel Pagination { get; set; }

        public IndexModel(IRocketService rocketService)
        {
            _rocketService = rocketService;
        }

        public async Task OnGet(int pageNumber = 1)
        {
            var (pagesCount, result) = await _rocketService.GetRocketsAsync(pageNumber);

            Rockets = result?.Select(r => r.ToRocketListItemViewModel()).ToList();

            Pagination = new PaginationViewModel(pageNumber, pagesCount, "/Rockets/Index");
        }
    }
}
