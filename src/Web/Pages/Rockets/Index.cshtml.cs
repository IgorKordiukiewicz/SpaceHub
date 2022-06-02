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

        public IndexModel(IRocketService rocketService)
        {
            _rocketService = rocketService;
        }

        public async Task OnGet()
        {
            var result = await _rocketService.GetRockets();
            Rockets = result?.Select(r => new RocketIndexViewModel(r)).ToList();
        }
    }
}
