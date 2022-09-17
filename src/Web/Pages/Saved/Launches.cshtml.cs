using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Web.Mapping;
using Web.ViewModels;

namespace Web.Pages.Saved;

[Authorize]
public class LaunchesModel : PageModel
{
    private readonly ISaveService _saveService;

    public List<LaunchIndexCardViewModel> Launches { get; set; }

    public PaginationViewModel Pagination { get; set; }

    public LaunchesModel(ISaveService saveService)
    {
        _saveService = saveService;
    }

    public void OnGet(int pageNumber = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Launches = _saveService.GetSavedLaunches(userId, pageNumber).Select(l => l.ToLaunchIndexCardViewModel()).ToList();

        var pagesCount = _saveService.GetSavedLaunchesPagesCount(userId);
        Pagination = new(pageNumber, pagesCount, "/Saved/Launches");
    }
}
