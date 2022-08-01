using Library.Api.Responses;
using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.ViewModels;
using Web.Mapping;
using System.Diagnostics;
using Library.Models;
using System.Security.Claims;

namespace Web.Pages.News;

public class IndexModel : PageModel
{
    private readonly IArticleService _articleService;
    private readonly ISaveService _saveService;

    public List<ArticleCardViewModel> Articles { get; set; }

    public PaginationViewModel Pagination { get; set; }

    [BindProperty]
    public string? SearchValue { get; set; }

    public IndexModel(IArticleService articleService, ISaveService saveService)
    {
        _articleService = articleService;
        _saveService = saveService;
    }
    
    public async Task OnGet(string? searchValue, int pageNumber = 1)
    {
        SearchValue = searchValue;

        var result = await _articleService.GetArticlesAsync(SearchValue, pageNumber);
        Articles = result.Select(a => a.ToArticleCardViewModel()).ToList();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        foreach (var article in Articles)
        {
            article.IsSaved = userId != null && _saveService.IsArticleSaved(userId, article.ApiId);
        }

        var pagesCount = await _articleService.GetPagesCountAsync(SearchValue);
        Pagination = new(pageNumber, pagesCount, "/News/Index", searchValue != null ? new() { { "searchValue", searchValue} } : null);
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("Index", new { searchValue = SearchValue, pageNumber = 1 });
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
