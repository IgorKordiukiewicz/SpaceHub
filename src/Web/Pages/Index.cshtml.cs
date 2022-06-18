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
            _articleService.Pagination.ItemsPerPage = 2;

            _launchService = launchService;
            _launchService.Pagination.ItemsPerPage = 3;

            _eventService = eventService;
            _eventService.Pagination.ItemsPerPage = 3;

            _saveService = saveService;
        }

        public async Task OnGet()
        {
            var articlesResult = await _articleService.GetArticlesAsync(null);
            Articles = articlesResult.Select(a => a.ToArticleCardViewModel()).ToList();

            foreach(var article in Articles)
            {
                article.IsSaved = _saveService.IsArticleSaved(article.ApiId);
            }
            
            var (_, launchesResult) = await _launchService.GetUpcomingLaunchesAsync(null);
            Launches = launchesResult.Select(l => l.ToLaunchIndexCardViewModel()).ToList();
            
            var (_, eventsResult) = await _eventService.GetUpcomingEventsAsync(null);
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