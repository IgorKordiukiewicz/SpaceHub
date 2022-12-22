using Infrastructure.Api;
using MediatR;

namespace Application.Features.News;

public record GetAllNewsQuery : IRequest<List<ArticleViewModel>>;

public class GetAllNewsHandler : IRequestHandler<GetAllNewsQuery, List<ArticleViewModel>>
{
    private readonly IArticleApi _articleApi;

    public GetAllNewsHandler(IArticleApi articleApi)
    {
        _articleApi = articleApi;
    }

    public async Task<List<ArticleViewModel>> Handle(GetAllNewsQuery request, CancellationToken cancellationToken)
    {
        return (await _articleApi.GetArticlesAsync()).Select(a => new ArticleViewModel
        {
            Title = a.Title,
        }).ToList();
    }
}

public class ArticleViewModel
{
    public string Title { get; set; }
}
