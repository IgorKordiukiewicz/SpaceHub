using Library.Api.Responses;
using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly IArticleService _articleService;

        public List<ArticleResponse> Articles { get; set; }

        public IndexModel(IArticleService articleService)
        {
            _articleService = articleService;
        }
        
        public async Task OnGet()
        {
            Articles = await _articleService.GetArticlesAsync();
        }
    }
}
