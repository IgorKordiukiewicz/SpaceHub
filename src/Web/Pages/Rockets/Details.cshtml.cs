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

        public RocketDetailsCardViewModel Rocket { get; set; }
        public AgencyCardViewModel Manufacturer { get; set; }

        public DetailsModel(IRocketService rocketService)
        {
            _rocketService = rocketService;
        }

        public async Task OnGet(int id)
        {
            var result = await _rocketService.GetRocketAsync(id);

            Rocket = result.ToRocketDetailsCardViewModel();
            Manufacturer = result.Details.Manufacturer.ToAgencyCardViewModel();
        }
    }
}
