using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.ViewModels;
using Web.Mapping;

namespace Web.Pages.Launches
{
    public class DetailsModel : PageModel
    {
        private readonly ILaunchService _launchService;
        private readonly IRocketService _rocketService;

        public LaunchViewModel Launch { get; set; }

        public DetailsModel(ILaunchService launchService, IRocketService rocketService)
        {
            _launchService = launchService;
            _rocketService = rocketService;
        }

        public async Task OnGet(string id)
        {
            var result = await _launchService.GetLaunchAsync(id);
            result.Rocket.Details.RankedProperties = await _rocketService.GetRocketRankedProperties(result.Rocket.ApiId);

            Launch = result.ToLaunchViewModel();
        }
    }
}
