using Library.Api.Responses;
using Library.Enums;
using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.ViewModels;
using Web.Mapping;

namespace Web.Pages.Launches;

public class IndexModel : PageModel
{
    private readonly ILaunchService _launchService;

    public List<LaunchIndexCardViewModel> Launches { get; set; }

    public PaginationViewModel Pagination { get; set; }

    [BindProperty]
    public string? SearchValue { get; set; }

    [BindProperty]
    public DateType LaunchDateType { get; set; }

    public IndexModel(ILaunchService launchService)
    {
        _launchService = launchService;
    }

    public async Task OnGet(DateType launchDateType, string? searchValue, int pageNumber = 1)
    {
        LaunchDateType = launchDateType;
        SearchValue = searchValue;
        
        var (pagesCount, result) = LaunchDateType == DateType.Upcoming ? 
            await _launchService.GetUpcomingLaunchesAsync(searchValue, pageNumber)
            : await _launchService.GetPreviousLaunchesAsync(searchValue, pageNumber);

        Launches = result?.Select(l => l.ToLaunchIndexCardViewModel()).ToList();

        Dictionary<string, string> paginationParameters = new()
        {
            { "launchDateType", LaunchDateType.ToString() }
        };
        if(SearchValue != null)
        {
            paginationParameters.Add("searchValue", SearchValue);
        }

        Pagination = new(pageNumber, pagesCount, "/Launches/Index", paginationParameters);
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("Index", new { launchDateType = LaunchDateType, searchValue = SearchValue, pageNumber = 1 });
    }
}
