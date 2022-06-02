using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.ViewModels;

namespace Web.Pages.Launches
{
    public class DetailsModel : PageModel
    {
        private readonly ILaunchService _launchService;

        public LaunchDetailViewModel Launch { get; set; }

        public DetailsModel(ILaunchService launchService)
        {
            _launchService = launchService;
        }

        public async Task OnGet(string id)
        {
            var result = await _launchService.GetLaunchAsync(id);
            Launch = new(result);
        }
    }
}
