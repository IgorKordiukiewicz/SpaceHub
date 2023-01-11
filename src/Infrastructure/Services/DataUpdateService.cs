using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SpaceHub.Domain.Enums;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Api.Responses;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Enums;
using System.Globalization;
using System.Xml.Linq;

namespace SpaceHub.Infrastructure.Services;

// should it be here or moved to Application/Features
// maybe the background service should call mediatr which has handlers for updateArticles, Launches, etc
public class DataUpdateService : BackgroundService
{
    private readonly IArticleApi _articleApi;
    private readonly DbContext _db;
    private readonly PeriodicTimer _timer;
    private readonly ILaunchApi _launchApi;

    public DataUpdateService(IArticleApi articleApi, DbContext db, IOptions<InfrastructureSettings> settingsOptions, ILaunchApi launchApi)
    {
        _articleApi = articleApi;
        _db = db;

        _timer = new PeriodicTimer(TimeSpan.FromMinutes(settingsOptions.Value.DataUpdateEveryXMinutes));
        _launchApi = launchApi;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(await _timer.WaitForNextTickAsync(stoppingToken)
            && !stoppingToken.IsCancellationRequested)
        {
            //await UpdateArticles();
            //await UpdateLaunches();
        }
    }
}
