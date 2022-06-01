using Library.Api.Responses;
using Library.Enums;
using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.ViewModels;

namespace Web.Pages.Launches
{
    public class IndexModel : PageModel
    {
        private readonly ILaunchService _launchService;

        public List<LaunchIndexViewModel>? Launches { get; set; }

        public IndexModel(ILaunchService launchService)
        {
            _launchService = launchService;
        }

        public async Task OnGet(LaunchDateType launchDateType)
        {
            var result = launchDateType == LaunchDateType.Upcoming ? 
                await _launchService.GetUpcomingLaunchesAsync()
                : await _launchService.GetPreviousLaunchesAsync();
            Launches = result?.Select(l => new LaunchIndexViewModel(l)).ToList();
        }
    }
}
