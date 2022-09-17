using Library.Data;
using Library.Mapping;
using Library.Models;
using Library.Utils;
using Library.Services.Interfaces;

namespace Library.Services;

public class SaveService : ISaveService
{
    private readonly AppDbContext _context;

    public SaveService(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveArticleAsync(string userId, Article article)
    {
        var articleEntity = article.ToEntity();
        articleEntity.UserId = userId;

        await _context.Articles.AddAsync(articleEntity);
        await _context.SaveChangesAsync();
    }

    public async Task UnsaveArticleAsync(string userId, int articleId)
    {
        var articleEntity = _context.Articles.FirstOrDefault(a => a.UserId == userId && a.ApiId == articleId);
        if(articleEntity is not null)
        {
            _context.Articles.Remove(articleEntity);
            await _context.SaveChangesAsync();
        }
    }

    public bool IsArticleSaved(string userId, int articleId)
    {
        return _context.Articles.Any(a => a.UserId == userId && a.ApiId == articleId);
    }

    public List<Article> GetSavedArticles(string userId, int pageNumber, int itemsPerPage)
    {
        int offset = Pagination.GetOffset(pageNumber, itemsPerPage);
        return _context.Articles
            .Where(a => a.UserId == userId)
            .Select(a => a.ToModel())
            .Skip(offset)
            .Take(itemsPerPage)
            .ToList();
    }

    public int GetSavedArticlesPagesCount(string userId, int itemsPerPage)
    {
        return Pagination.GetPagesCount(_context.Articles.Where(a => a.UserId == userId).Count(), itemsPerPage);
    }

    public async Task SaveLaunchAsync(string userId, Launch launch)
    {
        var launchEntity = launch.ToEntity();
        launchEntity.UserId = userId;

        await _context.Launches.AddAsync(launchEntity);
        await _context.SaveChangesAsync();
    }

    public async Task UnsaveLaunchAsync(string userId, string launchId)
    {
        var launchEntity = _context.Launches.FirstOrDefault(l => l.UserId == userId && l.ApiId == launchId);
        if(launchEntity is not null)
        {
            _context.Launches.Remove(launchEntity);
            await _context.SaveChangesAsync();
        }
    }

    public bool IsLaunchSaved(string userId, string launchId)
    {
        return _context.Launches.Any(l => l.UserId == userId && l.ApiId == launchId);
    }

    public List<Launch> GetSavedLaunches(string userId, int pageNumber, int itemsPerPage)
    {
        int offset = Pagination.GetOffset(pageNumber, itemsPerPage);
        return _context.Launches
            .Where(l => l.UserId == userId)
            .Select(l => l.ToModel())
            .Skip(offset)
            .Take(itemsPerPage)
            .ToList();
    }

    public int GetSavedLaunchesPagesCount(string userId, int itemsPerPage)
    {
        return Pagination.GetPagesCount(_context.Launches.Where(l => l.UserId == userId).Count(), itemsPerPage);
    }

    public async Task SaveEventAsync(string userId, Event event_)
    {
        var eventEntity = event_.ToEntity();
        eventEntity.UserId = userId;

        await _context.Events.AddAsync(eventEntity);
        await _context.SaveChangesAsync();
    }

    public async Task UnsaveEventAsync(string userId, int eventId)
    {
        var eventEntity = _context.Events.FirstOrDefault(e => e.UserId == userId && e.ApiId == eventId);
        if (eventEntity is not null)
        {
            _context.Events.Remove(eventEntity);
            await _context.SaveChangesAsync();
        }
    }

    public bool IsEventSaved(string userId, int eventId)
    {
        return _context.Events.Any(e => e.UserId == userId && e.ApiId == eventId);
    }

    public List<Event> GetSavedEvents(string userId, int pageNumber, int itemsPerPage)
    {
        int offset = Pagination.GetOffset(pageNumber, itemsPerPage);
        return _context.Events
            .Where(e => e.UserId == userId)
            .Select(e => e.ToModel())
            .Skip(offset)
            .Take(itemsPerPage)
            .ToList();
    }

    public int GetSavedEventsPagesCount(string userId, int itemsPerPage)
    {
        return Pagination.GetPagesCount(_context.Events.Where(l => l.UserId == userId).Count(), itemsPerPage);
    }
}
