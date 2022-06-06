using Library.Api.Responses;
using Library.Enums;
using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.ViewModels;
using Web.Mapping;

namespace Web.Pages.Launches
{
    public class IndexModel : PageModel
    {
        private readonly ILaunchService _launchService;

        public List<LaunchCardViewModel>? Launches { get; set; }

        public PaginationViewModel Pagination { get; set; }

        public IndexModel(ILaunchService launchService)
        {
            _launchService = launchService;
        }

        public async Task OnGet(LaunchDateType launchDateType, int pageNumber = 1)
        {
            var (pagesCount, result) = launchDateType == LaunchDateType.Upcoming ? 
                await _launchService.GetUpcomingLaunchesAsync(pageNumber)
                : await _launchService.GetPreviousLaunchesAsync(pageNumber);

            Launches = result?.Select(l => l.ToLaunchCardViewModel()).ToList();

            Pagination = new(pageNumber, pagesCount, "/Launches/Index", new() { { "launchDateType", launchDateType.ToString() } });
        }
    }
}
