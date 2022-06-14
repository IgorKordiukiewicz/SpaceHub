using Library.Enums;
using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Mapping;
using Web.ViewModels;

namespace Web.Pages.Rockets
{
    public class RankingsModel : PageModel
    {
        private readonly IRocketService _rocketService;

        public Dictionary<RocketRankedPropertyType, List<RocketRankedPropertyViewModel>> RankedProperties { get; set; } = new();

        public RankingsModel(IRocketService rocketService)
        {
            _rocketService = rocketService;
        }
        
        public async Task OnGet()
        {
            var result = await _rocketService.GetRocketRankedPropertiesRankingsAsync(10);
            foreach(var (propertyType, properties) in result)
            {
                RankedProperties.Add(propertyType, properties.Select(p => p.ToRocketRankedPropertyViewModel()).ToList());
            }
        }
    }
}
