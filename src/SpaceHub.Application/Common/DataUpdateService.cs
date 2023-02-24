using OneOf;
using Refit;
using SpaceHub.Application.Errors;
using SpaceHub.Infrastructure.Api.Responses;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Enums;

namespace SpaceHub.Application.Common;

public class DataUpdateService<TResponse, TResponseItem, TModel, TId>
{
    public required Func<Task<IApiResponse<MultiElementResponse>>> GetItemsCountFunc { get; init; }
    public required Func<int, Task<IApiResponse<TResponse>>> GetItemsFunc { get; init; }
    public required Func<TResponseItem, UpdateOneModel<TModel>> UpdateModelFunc { get; init; }
    public required Func<TResponseItem, InsertOneModel<TModel>> InsertModelFunc { get; init; }
    public required Func<TResponseItem, TId> ResponseItemIdSelector { get; init; }
    public required Func<TResponse, IEnumerable<TResponseItem>> ResponseItemsSelector { get; init; }
    public required Func<DbContext, IMongoCollection<TModel>> CollectionSelector { get; init; }
    public required HashSet<TId> ExistingIds { get; set; }
    public required int MaxItemsPerRequest { get; init; }
    public required ECollection CollectionType { get; init; }
    public required DbContext Db { get; init; }

    public async Task<Result> Handle()
    {
        var itemsResult = await GetItemsFromApi();
        if(!itemsResult.TryPickT0(out var items, out var apiError))
        {
            return apiError;
        }

        if(!items.Any())
        {
            return Result.Ok();
        }

        var writes = new List<WriteModel<TModel>>();
        foreach(var item in items)
        {
            if (ExistingIds.Contains(ResponseItemIdSelector(item)))
            {
                writes.Add(UpdateModelFunc(item));
            }
            else
            {
                writes.Add(InsertModelFunc(item));
            }
        }

        if(writes.Any())
        {
            _ = await CollectionSelector(Db).BulkWriteAsync(writes);
        }

        _ = await Db.CollectionsLastUpdates.UpdateOneAsync(
            x => x.CollectionType == CollectionType,
            Builders<CollectionLastUpdateModel>.Update.Set(x => x.LastUpdate, DateTime.UtcNow));

        return Result.Ok();
    }

    public async Task<OneOf<List<TResponseItem>, ApiError>> GetItemsFromApi()
    {
        var countResponse = await GetItemsCountFunc();
        if (!countResponse.GetContentOrError().TryPickT0(out var countResponseContent, out var countResponseError))
        {
            return countResponseError;
        }

        var count = countResponseContent.Count;
        int requestsRequired = ApiHelpers.GetRequiredRequestsCount(count, MaxItemsPerRequest);

        var tasks = new List<Task<IApiResponse<TResponse>>>();
        for (int i = 0; i < requestsRequired; ++i)
        {
            tasks.Add(GetItemsFunc(i));
        }
        await Task.WhenAll(tasks);

        var result = new List<TResponseItem>();
        foreach (var task in tasks)
        {
            if (!task.Result.GetContentOrError().TryPickT0(out var itemResponseContent, out var itemResponseError))
            {
                return itemResponseError;
            }

            result.AddRange(ResponseItemsSelector(itemResponseContent));
        }

        return result;
    }
}
