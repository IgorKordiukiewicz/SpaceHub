using Refit;
using SpaceHub.Infrastructure.Api.Responses;

namespace SpaceHub.Infrastructure.Api;

public interface IArticleApi
{
    [Get("/articles?_publishedAt_gt={startDate}&_publishedAt_lt={endDate}&_limit=1000")]
    Task<IReadOnlyCollection<ArticleResponse>> GetArticlesPublishedBetweenAsync(string startDate, string endDate);
}
