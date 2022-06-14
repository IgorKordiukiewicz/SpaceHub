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

        public LaunchDetailsCardViewModel Launch { get; set; }
        public AgencyCardViewModel Agency { get; set; }
        public RocketDetailsCardViewModel Rocket { get; set; }
        public PadCardViewModel Pad { get; set; }
        public List<SpaceProgramCardViewModel> Programs { get; set; }

        public DetailsModel(ILaunchService launchService)
        {
            _launchService = launchService;
        }

        public async Task OnGet(string id)
        {
            var result = await _launchService.GetLaunchAsync(id);

            Launch = result.ToLaunchDetailsCardViewModel();
            Agency = result.Agency.ToAgencyCardViewModel();
            Rocket = result.Rocket.ToRocketDetailsCardViewModel();
            Pad = result.Pad.ToPadCardViewModel();
            Programs = result.Programs.Select(p => p.ToSpaceProgramCardViewModel()).ToList();
        }
    }
}
