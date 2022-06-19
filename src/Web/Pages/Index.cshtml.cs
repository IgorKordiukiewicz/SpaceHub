using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Mapping;
using Web.ViewModels;

namespace Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IArticleService _articleService;
        private readonly ILaunchService _launchService;
        private readonly IEventService _eventService;
        private readonly ISaveService _saveService;

        public List<ArticleCardViewModel> Articles { get; set; }
        public List<LaunchIndexCardViewModel> Launches { get; set; }
        public List<EventIndexCardViewModel> Events { get; set; }

        public IndexModel(IArticleService articleService, ILaunchService launchService, IEventService eventService, ISaveService saveService)
        {
            _articleService = articleService;
            _launchService = launchService;
            _eventService = eventService;
            _saveService = saveService;
        }

        public async Task OnGet()
        {
            var articlesResult = await _articleService.GetArticlesAsync(null, 1, 2);
            Articles = articlesResult.Select(a => a.ToArticleCardViewModel()).ToList();

            foreach(var article in Articles)
            {
                article.IsSaved = _saveService.IsArticleSaved(article.ApiId);
            }
            
            var (_, launchesResult) = await _launchService.GetUpcomingLaunchesAsync(null, 1, 3);
            Launches = launchesResult.Select(l => l.ToLaunchIndexCardViewModel()).ToList();
            
            var (_, eventsResult) = await _eventService.GetUpcomingEventsAsync(null, 1, 3);
            Events = eventsResult.Select(e => e.ToEventIndexCardViewModel()).ToList();
        }

        public async Task<IActionResult> OnPostToggleSave(int articleId)
        {
            if (_saveService.IsArticleSaved(articleId))
            {
                await _saveService.UnsaveArticleAsync(articleId);
                return Partial("_SaveToggle", false);
            }
            else
            {
                var article = await _articleService.GetArticleAsync(articleId);
                await _saveService.SaveArticleAsync(article);
                return Partial("_SaveToggle", true);
            }
        }
    }
}