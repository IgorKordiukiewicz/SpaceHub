using Library.Api.Responses;
using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.ViewModels;

namespace Web.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly IArticleService _articleService;

        public List<ArticleViewModel> Articles { get; set; }

        [BindProperty]
        public string? SearchValue { get; set; }

        [BindProperty]
        public int PageNumber { get; set; }

        public int PagesCount { get; set; } = 1;

        public IndexModel(IArticleService articleService)
        {
            _articleService = articleService;
        }
        
        public async Task OnGet(string? searchValue, int pageNumber = 1)
        {
            SearchValue = searchValue;
            PageNumber = pageNumber;

            PagesCount = await _articleService.GetPagesCount(SearchValue);

            var result = await _articleService.GetArticlesAsync(SearchValue, PageNumber);
            Articles = result.Select(a => new ArticleViewModel(a)).ToList();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Index", new { searchValue = SearchValue, pageNumber = 1 });
        }
    }
}
