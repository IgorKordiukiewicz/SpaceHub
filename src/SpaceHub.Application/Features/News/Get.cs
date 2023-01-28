using FluentResults;
using FluentValidation;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SpaceHub.Application.Common;
using SpaceHub.Application.Errors;
using SpaceHub.Application.Features.Launches;
using SpaceHub.Contracts.ViewModels;
using SpaceHub.Domain;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;

namespace SpaceHub.Application.Features.News;

public record GetNewsQuery(string SearchValue, int PageNumber, int ItemsPerPage) : IRequest<Result<ArticlesVM>>;

public class GetNewsQueryValidator : AbstractValidator<GetNewsQuery>
{
    public GetNewsQueryValidator()
    {
        RuleFor(x => x.PageNumber).NotNull().GreaterThanOrEqualTo(1);
        RuleFor(x => x.ItemsPerPage).NotNull().GreaterThanOrEqualTo(1);
        RuleFor(x => x.SearchValue).NotNull();
    }
}

internal class GetNewsHandler : IRequestHandler<GetNewsQuery, Result<ArticlesVM>>
{
    private readonly DbContext _db;

    public GetNewsHandler(DbContext db)
    {
        _db = db;
    }

    public async Task<Result<ArticlesVM>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
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

        return Result.Ok(new ArticlesVM(articlesViewModels, totalPagesCount));
    }
}
