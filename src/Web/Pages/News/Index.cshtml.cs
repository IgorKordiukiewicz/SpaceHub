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

        public IndexModel(IArticleService articleService)
        {
            _articleService = articleService;
        }
        
        public async Task OnGet()
        {
            var result = await _articleService.GetArticlesAsync();
            Articles = result.Select(a => new ArticleViewModel(a)).ToList();
        }
    }
}
