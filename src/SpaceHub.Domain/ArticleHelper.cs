namespace SpaceHub.Domain;

public static class ArticleHelper
{
    public static bool ArticleMatchesSearchCriteria(string searchValue, string title, string summary)
    {
        // TODO: Improve search, because simple contains might include unwanted cases,
        // e.g. if search = 'mars', and article contains word 'marshmallow' the article will be included
        return title.Contains(searchValue, StringComparison.OrdinalIgnoreCase)
            || summary.Contains(searchValue, StringComparison.OrdinalIgnoreCase);
    }
}
