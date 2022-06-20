using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Mapping;
using Web.ViewModels;

namespace Web.Pages.Saved
{
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
            Articles = _saveService.GetSavedArticles(pageNumber).Select(a => a.ToArticleCardViewModel()).ToList();

            foreach (var article in Articles)
            {
                article.IsSaved = true;
            }

            var pagesCount = _saveService.GetSavedArticlesPagesCount();
            Pagination = new(pageNumber, pagesCount, "/Saved/News");
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
