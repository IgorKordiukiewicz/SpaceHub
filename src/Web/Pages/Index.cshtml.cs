using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Web.Mapping;
using Web.ViewModels;

namespace Web.Pages;

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

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        foreach (var article in Articles)
        {
            article.IsSaved = userId is not null && _saveService.IsArticleSaved(userId, article.ApiId);
        }
        
        var (_, launchesResult) = await _launchService.GetUpcomingLaunchesAsync(null, 1, 3);
        Launches = launchesResult.Select(l => l.ToLaunchIndexCardViewModel()).ToList();
        
        var (_, eventsResult) = await _eventService.GetUpcomingEventsAsync(null, 1, 3);
        Events = eventsResult.Select(e => e.ToEventIndexCardViewModel()).ToList();
    }

    public async Task<IActionResult> OnPostToggleSave(int articleId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (_saveService.IsArticleSaved(userId, articleId))
        {
            await _saveService.UnsaveArticleAsync(userId, articleId);
            return Partial("_SaveToggle", false);
        }
        else
        {
            var article = await _articleService.GetArticleAsync(articleId);
            await _saveService.SaveArticleAsync(userId, article);
            return Partial("_SaveToggle", true);
        }
    }
}
