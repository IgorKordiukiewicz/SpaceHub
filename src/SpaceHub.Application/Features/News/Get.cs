using SpaceHub.Application.Interfaces;
using SpaceHub.Contracts.Models;
using SpaceHub.Domain.Models;

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

internal sealed class GetNewsHandler : IRequestHandler<GetNewsQuery, Result<ArticlesVM>>
{
    private readonly IDbContext _db;

    public GetNewsHandler(IDbContext db)
    {
        _db = db;
    }

    public async Task<Result<ArticlesVM>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        var articles = await _db.Articles.AsQueryable()
            .OrderByDescending(x => x.PublishDate)
            .ToListAsync(cancellationToken);

        var filteredArticles = new List<Article>();
        foreach (var article in articles)
        {
            if(!article.MatchesSearch(request.SearchValue))
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
