using Library.Api.Responses;
using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.ViewModels;

namespace Web.Pages.Launches
{
    public class IndexModel : PageModel
    {
        private readonly ILaunchService _launchService;

        public List<LaunchViewModel> Launches { get; set; }

        public IndexModel(ILaunchService launchService)
        {
            _launchService = launchService;
        }

        public async Task OnGet()
        {
            var result = await _launchService.GetUpcomingLaunchesAsync();
            Launches = result.Select(l => new LaunchViewModel(l)).ToList();
        }
    }
}
