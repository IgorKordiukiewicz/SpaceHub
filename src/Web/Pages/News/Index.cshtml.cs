using Library.Api;
using Library.Api.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public List<ArticleResponse> Articles { get; set; }

        public IndexModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        public async Task OnGet()
        {
            Articles = await _apiClient.GetArticlesAsync();
        }
    }
}
