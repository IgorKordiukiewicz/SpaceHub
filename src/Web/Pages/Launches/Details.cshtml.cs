using Library.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.ViewModels;
using Web.Mapping;
using System.Security.Claims;

namespace Web.Pages.Launches;

public class DetailsModel : PageModel
{
    private readonly ILaunchService _launchService;
    private readonly ISaveService _saveService;

    public LaunchDetailsCardViewModel Launch { get; set; }
    public AgencyCardViewModel Agency { get; set; }
    public RocketDetailsCardViewModel Rocket { get; set; }
    public PadCardViewModel Pad { get; set; }
    public List<SpaceProgramCardViewModel> Programs { get; set; }

    public DetailsModel(ILaunchService launchService, ISaveService saveService)
    {
        _launchService = launchService;
        _saveService = saveService;
    }

    public async Task OnGet(string id)
    {
        var result = await _launchService.GetLaunchAsync(id);

        Launch = result.ToLaunchDetailsCardViewModel();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Launch.IsSaved = userId is not null && _saveService.IsLaunchSaved(userId, id);

        Agency = result.Agency.ToAgencyCardViewModel();
        Rocket = result.Rocket.ToRocketDetailsCardViewModel();
        Pad = result.Pad.ToPadCardViewModel();
        Programs = result.Programs.Select(p => p.ToSpaceProgramCardViewModel()).ToList();
    }

    public async Task<IActionResult> OnPostToggleSaveLaunch(string launchId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (_saveService.IsLaunchSaved(userId, launchId))
        {
            await _saveService.UnsaveLaunchAsync(userId, launchId);
            return Partial("_SaveToggle", false);
        }
        else
        {
            var launch = await _launchService.GetLaunchAsync(launchId);
            await _saveService.SaveLaunchAsync(userId, launch);
            return Partial("_SaveToggle", true);
        }
    }
}
