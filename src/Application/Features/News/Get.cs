using MediatR;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SpaceHub.Application.Common;
using SpaceHub.Contracts.ViewModels;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;

namespace SpaceHub.Application.Features.News;

public record GetNewsQuery(string SearchValue, int PageNumber, int ItemsPerPage) : IRequest<ArticlesVM>;

internal class GetNewsHandler : IRequestHandler<GetNewsQuery, ArticlesVM>
{
    private readonly IArticleApi _articleApi;
    private readonly IMemoryCache _cache;
    private readonly DbContext _db;

    public GetNewsHandler(IArticleApi articleApi, IMemoryCache cache, DbContext db)
    {
        _articleApi = articleApi;
        _cache = cache;
        _db = db;
    }

    public async Task<ArticlesVM> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        var offset = Pagination.GetOffset(request.PageNumber, request.ItemsPerPage);

        // TODO: To improve performance, maybe add where clause to pre-filter articles by search
        // e.g. Title/summary must contain searchValue, and then the queried models will be further filtered by more complex and accurate search?
        var articles = await _db.Articles.AsQueryable()
            .OrderByDescending(x => x.PublishDate)
            .ToListAsync();

        var filteredArticles = new List<ArticleModel>();
        foreach (var article in articles)
        {
            if(!ArticleMatchesSearchCriteria(request.SearchValue, article.Title, article.Summary))
            {
                continue;
            }

            filteredArticles.Add(article);
        }

        var totalArticlesCount = filteredArticles.Count;
        var totalPagesCount = Pagination.GetPagesCount((int)totalArticlesCount, request.ItemsPerPage);

        var articlesViewModels = filteredArticles
            .Skip(offset)
            .Take(request.ItemsPerPage)
            .Select(x => new ArticleVM
            {
                Title = x.Title,
                Summary = x.Summary,
                ImageUrl = x.ImageUrl,
                NewsSite = x.NewsSite,
                PublishDate = x.PublishDate,
                Url = x.Url
            }).ToList();

        return new ArticlesVM(articlesViewModels, totalPagesCount);
    }

    // TODO: Move to domain
    private bool ArticleMatchesSearchCriteria(string searchValue, string title, string summary)
    {
        // TODO: Improve search, because simple contains might include unwanted cases,
        // e.g. if search = 'mars', and article contains word 'marshmallow' the article will be included
        return title.Contains(searchValue, StringComparison.OrdinalIgnoreCase) 
            || summary.Contains(searchValue, StringComparison.OrdinalIgnoreCase);
    }
}
