using Refit;

namespace Infrastructure.Api;

public interface IArticleApi
{
    [Get("/articles/?_title_contains={searchValue}&_limit={limit}&_start={start}")]
    Task<IEnumerable<ArticleResponse>> GetArticlesAsync(string? searchValue, int limit, int start = 0);

    [Get("/articles/count/?_title_contains={searchValue}")]
    Task<int> GetArticlesCountAsync(string? searchValue);
}
