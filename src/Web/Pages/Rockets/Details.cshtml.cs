using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.ViewModels;
using Web.Mapping;

namespace Web.Pages.Rockets
{
    public class DetailsModel : PageModel
    {
        private readonly IRocketService _rocketService;

        public RocketCardViewModel Rocket { get; set; }

        public DetailsModel(IRocketService rocketService)
        {
            _rocketService = rocketService;
        }

        public async Task OnGet(int id)
        {
            var result = await _rocketService.GetRocketAsync(id);
            result.Details.RankedProperties = await _rocketService.GetRocketRankedProperties(id);

            Rocket = result.ToRocketCardViewModel();
        }
    }
}
