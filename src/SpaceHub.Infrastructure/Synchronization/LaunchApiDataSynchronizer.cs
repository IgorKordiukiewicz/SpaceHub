using FluentResults;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Refit;
using SpaceHub.Infrastructure.Api.Responses;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Enums;
using OneOf;
using SpaceHub.Infrastructure.Errors;
using SpaceHub.Infrastructure.Synchronization.Interfaces;
using SpaceHub.Infrastructure.Extensions;

namespace SpaceHub.Infrastructure.Synchronization;

public abstract class LaunchApiDataSynchronizer<TResponse, TResponseItem, TModel, TId> : IDataSynchronizer<TModel> 
    where TModel : class
{
    protected readonly DbContext _db;
    protected readonly ILaunchApi _api;

    protected string _startDateParameter = string.Empty;
    protected string _endDateParameter = string.Empty;
    protected HashSet<TId> _existingsIds = new();

    protected abstract ECollection CollectionType { get; }
    protected abstract int MaxItemsPerRequest { get; }

    public LaunchApiDataSynchronizer(DbContext db, ILaunchApi api)
    {
        _db = db;
        _api = api;
    }

    public async Task<Result> Synchronize()
    {
        var lastUpdateTime = await _db.CollectionsLastUpdates.AsQueryable()
            .Where(x => x.CollectionType == CollectionType)
            .Select(x => x.LastUpdate)
            .SingleAsync();

        _startDateParameter = lastUpdateTime.ToQueryParameter();
        _endDateParameter = DateTime.UtcNow.ToQueryParameter();

        var itemsResult = await GetItemsFromApi();
        if (!itemsResult.TryPickT0(out var items, out var apiError))
        {
            return apiError;
        }

        if (!items.Any())
        {
            return Result.Ok();
        }

        _existingsIds = CreateExistingIdsHashSet();

        var writes = new List<WriteModel<TModel>>();
        foreach (var item in items)
        {
            if (_existingsIds.Contains(GetResponseItemId(item)))
            {
                writes.Add(CreateUpdateModel(item));
            }
            else
            {
                writes.Add(CreateInsertModel(item));
            }
        }

        if (writes.Any())
        {
            _ = await GetCollection(_db).BulkWriteAsync(writes);
        }
        _ = await _db.CollectionsLastUpdates.UpdateOneAsync(
            x => x.CollectionType == CollectionType,
            Builders<CollectionLastUpdateModel>.Update.Set(x => x.LastUpdate, DateTime.UtcNow));

        return Result.Ok();
    }

    private async Task<OneOf<List<TResponseItem>, ApiError>> GetItemsFromApi()
    {
        var countResponse = await GetItemsCount();
        if (!countResponse.GetContentOrError().TryPickT0(out var countResponseContent, out var countResponseError))
        {
            return countResponseError;
        }

        var count = countResponseContent.Count;
        int requestsRequired = ApiHelpers.GetRequiredRequestsCount(count, MaxItemsPerRequest);

        var tasks = new List<Task<IApiResponse<TResponse>>>();
        for (int i = 0; i < requestsRequired; ++i)
        {
            tasks.Add(GetItems(i));
        }
        await Task.WhenAll(tasks);

        var result = new List<TResponseItem>();
        foreach (var task in tasks)
        {
            if (!task.Result.GetContentOrError().TryPickT0(out var itemResponseContent, out var itemResponseError))
            {
                return itemResponseError;
            }

            result.AddRange(SelectResponseItems(itemResponseContent));
        }

        return result;
    }

    protected abstract HashSet<TId> CreateExistingIdsHashSet();

    protected abstract TId GetResponseItemId(TResponseItem item);

    protected abstract Task<IApiResponse<MultiElementResponse>> GetItemsCount();

    protected abstract Task<IApiResponse<TResponse>> GetItems(int count);

    protected abstract IReadOnlyList<TResponseItem> SelectResponseItems(TResponse response);

    protected abstract IMongoCollection<TModel> GetCollection(DbContext db);

    protected abstract UpdateOneModel<TModel> CreateUpdateModel(TResponseItem item);

    protected abstract InsertOneModel<TModel> CreateInsertModel(TResponseItem item);
}
