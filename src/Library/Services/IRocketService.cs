using Library.Enums;
using Library.Models;

namespace Library.Services;

public interface IRocketService
{
    Task<(int, List<Rocket>)> GetRocketsAsync(string? searchValue, int pageNumber = 1, int itemsPerPage = 12);

    Task<Rocket> GetRocketAsync(int id);

    Task SetRocketRankedPropertiesAsync(Rocket rocket);

    Task<Dictionary<RocketRankedPropertyType, List<RocketRankedProperty>>> GetRocketRankedPropertiesRankingsAsync(int limit);
}
