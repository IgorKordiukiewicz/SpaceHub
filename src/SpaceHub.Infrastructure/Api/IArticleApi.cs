using Refit;
using SpaceHub.Infrastructure.Api.Responses;

namespace SpaceHub.Infrastructure.Api;

public interface IArticleApi
{
    [Get("/articles?_publishedAt_gt={startDate}&_publishedAt_lt={endDate}&_limit=100000")]
    Task<IApiResponse<IReadOnlyList<ArticleResponse>>> GetArticlesPublishedBetween(string startDate, string endDate);
}
