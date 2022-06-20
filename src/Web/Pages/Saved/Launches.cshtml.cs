using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Mapping;
using Web.ViewModels;

namespace Web.Pages.Saved
{
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
            Launches = _saveService.GetSavedLaunches(pageNumber).Select(l => l.ToLaunchIndexCardViewModel()).ToList();

            var pagesCount = _saveService.GetSavedLaunchesPagesCount();
            Pagination = new(pageNumber, pagesCount, "/Saved/Launches");
        }
    }
}
