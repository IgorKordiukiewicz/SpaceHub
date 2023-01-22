using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SpaceHub.Application.Common;
using SpaceHub.Application.Exceptions;
using SpaceHub.Contracts.ViewModels;
using SpaceHub.Domain;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;

namespace SpaceHub.Application.Features.News;

public record GetNewsQuery(string SearchValue, int PageNumber, int ItemsPerPage) : IRequest<Result<ArticlesVM>>;

internal class GetNewsHandler : IRequestHandler<GetNewsQuery, Result<ArticlesVM>>
{
    private readonly DbContext _db;

    public GetNewsHandler(DbContext db)
    {
        _db = db;
    }

    public async Task<Result<ArticlesVM>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        if (request.PageNumber <= 0)
        {
            return new Result<ArticlesVM>(new ValidationException("Page number has to be > 0."));
        }
        if (request.ItemsPerPage <= 0)
        {
            return new Result<ArticlesVM>(new ValidationException("Items per page have to be > 0."));
        }

        var offset = Pagination.GetOffset(request.PageNumber, request.ItemsPerPage);

        // TODO: To improve performance, maybe add where clause to pre-filter articles by search
        // e.g. Title/summary must contain searchValue, and then the queried models will be further filtered by more complex and accurate search?
        var articles = await _db.Articles.AsQueryable()
            .OrderByDescending(x => x.PublishDate)
            .ToListAsync();

        var filteredArticles = new List<ArticleModel>();
        foreach (var article in articles)
        {
            if(!ArticleHelper.ArticleMatchesSearchCriteria(request.SearchValue, article.Title, article.Summary))
            {
                continue;
            }

            filteredArticles.Add(article);
        }

        var totalArticlesCount = filteredArticles.Count;
        var totalPagesCount = Pagination.GetPagesCount(totalArticlesCount, request.ItemsPerPage);

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
}
