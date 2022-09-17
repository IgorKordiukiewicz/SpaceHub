using Library.Models;

namespace Library.Services.Interfaces;

public interface IArticleService
{
    Task<List<Article>> GetArticlesAsync(string? searchValue, int pageNumber = 1, int itemsPerPage = 10);

    Task<Article> GetArticleAsync(int id);

    Task<int> GetPagesCountAsync(string? searchValue, int itemsPerPage = 10);
}
