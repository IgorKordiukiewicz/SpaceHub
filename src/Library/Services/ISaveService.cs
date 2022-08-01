using Library.Models;

namespace Library.Services;

public interface ISaveService
{
    Task SaveArticleAsync(string userId, Article article);
    Task UnsaveArticleAsync(string userId, int articleId);
    bool IsArticleSaved(string userId, int articleId);
    List<Article> GetSavedArticles(string userId, int pageNumber = 1, int itemsPerPage = 10);
    int GetSavedArticlesPagesCount(string userId, int itemsPerPage = 10);

    Task SaveLaunchAsync(string userId, Launch launch);
    Task UnsaveLaunchAsync(string userId, string launchId);
    bool IsLaunchSaved(string userId, string launchId);
    List<Launch> GetSavedLaunches(string userId, int pageNumber = 1, int itemsPerPage = 12);
    int GetSavedLaunchesPagesCount(string userId, int itemsPerPage = 12);

    Task SaveEventAsync(string userId, Event event_);
    Task UnsaveEventAsync(string userId, int eventId);
    bool IsEventSaved(string userId, int eventId);
    List<Event> GetSavedEvents(string userId, int pageNumber = 1, int itemsPerPage = 12);
    int GetSavedEventsPagesCount(string userId, int itemsPerPage = 12);
}
