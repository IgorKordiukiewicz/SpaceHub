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
            Launch.IsSaved = _saveService.IsLaunchSaved(id);

            Agency = result.Agency.ToAgencyCardViewModel();
            Rocket = result.Rocket.ToRocketDetailsCardViewModel();
            Pad = result.Pad.ToPadCardViewModel();
            Programs = result.Programs.Select(p => p.ToSpaceProgramCardViewModel()).ToList();
        }

        public async Task<IActionResult> OnPostToggleSave(string launchId)
        {
            if(_saveService.IsLaunchSaved(launchId))
            {
                await _saveService.UnsaveLaunchAsync(launchId);
                return Partial("_SaveToggle", false);
            }
            else
            {
                var launch = await _launchService.GetLaunchAsync(launchId);
                await _saveService.SaveLaunchAsync(launch);
                return Partial("_SaveToggle", true);
            }
        }
    }
}
