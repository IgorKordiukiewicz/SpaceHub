using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Web.Mapping;
using Web.ViewModels;

namespace Web.Pages.Saved;

[Authorize]
public class NewsModel : PageModel
{
    private readonly IArticleService _articleService;
    private readonly ISaveService _saveService;

    public List<ArticleCardViewModel> Articles { get; set; }

    public PaginationViewModel Pagination { get; set; }

    public NewsModel(IArticleService articleService, ISaveService saveService)
    {
        _articleService = articleService;
        _saveService = saveService;
    }

    public void OnGet(int pageNumber = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Articles = _saveService.GetSavedArticles(userId, pageNumber).Select(a => a.ToArticleCardViewModel()).ToList();

        foreach (var article in Articles)
        {
            article.IsSaved = true;
        }

        var pagesCount = _saveService.GetSavedArticlesPagesCount(userId);
        Pagination = new(pageNumber, pagesCount, "/Saved/News");
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
