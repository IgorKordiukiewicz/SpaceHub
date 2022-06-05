using Library.Api.Requests;
using Library.Api.Responses;
using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.ViewModels;
using Web.Mapping;

namespace Web.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly IArticleService _articleService;

        public List<ArticleViewModel> Articles { get; set; }

        public PaginationViewModel Pagination { get; set; }

        [BindProperty]
        public string? SearchValue { get; set; }

        public IndexModel(IArticleService articleService)
        {
            _articleService = articleService;
        }
        
        public async Task OnGet(string? searchValue, int pageNumber = 1)
        {
            SearchValue = searchValue;

            var result = await _articleService.GetArticlesAsync(new ArticleRequest { SearchValue = SearchValue, PageNumber = pageNumber });
            Articles = result.Select(a => a.ToArticleViewModel()).ToList();

            var pagesCount = await _articleService.GetPagesCountAsync(SearchValue);
            Pagination = new PaginationViewModel(pageNumber, pagesCount, "/News/Index", searchValue != null ? new() { { "searchValue", searchValue} } : null);
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Index", new { searchValue = SearchValue, pageNumber = 1 });
        }
    }
}
