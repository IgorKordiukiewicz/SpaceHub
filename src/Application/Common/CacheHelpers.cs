namespace Application.Common;

public static class Helpers
{
    public static string GetCacheKeyForRequestWithPages(string name, string? searchValue, int offset, int itemsPerPage)
    {
        return $"{name}_{searchValue}_{offset}_{itemsPerPage}";
    }
}
