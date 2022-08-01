using Library.Models;

namespace Library.Services;

public interface ILaunchService
{
    Task<(int, List<Launch>)> GetUpcomingLaunchesAsync(string? searchValue, int pageNumber = 1, int itemsPerPage = 12);

    Task<(int, List<Launch>)> GetPreviousLaunchesAsync(string? searchValue, int pageNumber = 1, int itemsPerPage = 12);

    Task<Launch> GetLaunchAsync(string id);
}
