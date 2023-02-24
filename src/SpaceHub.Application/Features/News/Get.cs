using SpaceHub.Contracts.Utils;
using SpaceHub.Domain;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;

namespace SpaceHub.Application.Features.News;

public record GetNewsQuery(string SearchValue, Pagination Pagination) : IRequest<Result<ArticlesVM>>;

public class GetNewsQueryValidator : AbstractValidator<GetNewsQuery>
{
    public GetNewsQueryValidator()
    {
        RuleFor(x => x.SearchValue).NotNull();
        RuleFor(x => x.Pagination).SetValidator(new PaginationValidator());
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
        var totalPagesCount = request.Pagination.GetPagesCount(totalArticlesCount);

        var articlesViewModels = filteredArticles
            .Skip(request.Pagination.Offset)
            .Take(request.Pagination.ItemsPerPage)
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
