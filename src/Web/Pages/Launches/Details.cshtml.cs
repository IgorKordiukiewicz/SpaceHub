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

        public LaunchViewModel Launch { get; set; }

        public DetailsModel(ILaunchService launchService)
        {
            _launchService = launchService;
        }

        public async Task OnGet(string id)
        {
            var result = await _launchService.GetLaunchAsync(id);
            Launch = result.ToLaunchViewModel();
        }
    }
}
