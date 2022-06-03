using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.ViewModels;

namespace Web.Pages.Rockets
{
    public class DetailsModel : PageModel
    {
        private readonly IRocketService _rocketService;

        public RocketDetailViewModel Rocket { get; set; }

        public DetailsModel(IRocketService rocketService)
        {
            _rocketService = rocketService;
        }

        public async Task OnGet(int id)
        {
            var result = await _rocketService.GetRocketAsync(id);
            Rocket = new(result);
        }
    }
}
