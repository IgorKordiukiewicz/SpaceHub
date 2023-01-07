using Refit;
using SpaceHub.Infrastructure.Api.Responses;

namespace SpaceHub.Infrastructure.Api;

public interface IArticleApi
{
    [Get("/articles/?_title_contains={searchValue}&_limit={limit}&_start={start}")]
    Task<IEnumerable<ArticleResponse>> GetArticlesAsync(string? searchValue, int limit, int start = 0);

    [Get("/articles/count/?_title_contains={searchValue}")]
    Task<int> GetArticlesCountAsync(string? searchValue);

    [Get("/articles?_publishedAt_gt={startDate}&_publishedAt_lt={endDate}&_limit=1000")]
    Task<IReadOnlyCollection<ArticleResponse>> GetArticlesPublishedBetweenAsync(string startDate, string endDate);
}
