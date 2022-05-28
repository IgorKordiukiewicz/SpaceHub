using Library.Api.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Library.Api
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient = new();

        public ApiClient()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<ArticleResponse>> GetArticlesAsync()
        {
            var response = await _httpClient.GetAsync("https://api.spaceflightnewsapi.net/v3/articles");
            var responseText = await response.Content.ReadAsStringAsync();

            List<ArticleResponse> articles = JsonConvert.DeserializeObject<List<ArticleResponse>>(responseText);

            return articles;
        }
    }
}
