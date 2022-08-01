using Library.Api;
using Library.Models;
using Library.Mapping;
using Library.Enums;
using Library.Api.Responses;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Services;

public class RocketService : IRocketService
{
    private readonly ILaunchApi _launchApi;
    private readonly IMemoryCache _cache;
    private Dictionary<int, Dictionary<RocketRankedPropertyType, int?>>? _rocketRankedProperties;
    private Dictionary<RocketRankedPropertyType, List<RocketRankedProperty>>? _rankedPropertiesRankings;

    public RocketService(ILaunchApi launchApi, IMemoryCache cache)
    {
        _launchApi = launchApi;
        _cache = cache;
    }

    public async Task<(int, List<Rocket>)> GetRocketsAsync(string? searchValue, int pageNumber, int itemsPerPage)
    {
        var (pagesCount, result) = await Helpers.GetApiResponseWithSearchAndPagination("rockets", _launchApi.GetRocketsAsync, 
            searchValue, pageNumber, itemsPerPage, _cache);

        return (pagesCount, result.Rockets.Select(r => r.ToModel()).ToList());
    }

    public async Task<Rocket> GetRocketAsync(int id)
    {
        var result = await _cache.GetOrCreateAsync("rocket" + id.ToString(), async entry =>
        {
            return await _launchApi.GetRocketAsync(id);
        });

        var rocket = result.ToModel();
        await SetRocketRankedPropertiesAsync(rocket);

        return rocket;
    }

    public async Task SetRocketRankedPropertiesAsync(Rocket rocket)
    {
        if (_rocketRankedProperties == null)
        {
            await InitializeRocketRankedPropertiesAsync();
        }

        rocket.Details.RankedProperties = _rocketRankedProperties.GetValueOrDefault(rocket.ApiId);
    }

    public async Task<Dictionary<RocketRankedPropertyType, List<RocketRankedProperty>>> GetRocketRankedPropertiesRankingsAsync(int limit)
    {
        if(_rankedPropertiesRankings == null)
        {
            await InitializeRocketRankedPropertiesAsync();
        }

        Dictionary<RocketRankedPropertyType, List<RocketRankedProperty>> result = new();
        foreach(var (propertyType, properties) in _rankedPropertiesRankings)
        {
            result.Add(propertyType, properties.Take(limit).ToList());
        }
        return result;
    }

    private async Task InitializeRocketRankedPropertiesAsync()
    {
        _rocketRankedProperties = new();

        var rockets = await GetAllRocketsDetailAsync();
        _rankedPropertiesRankings = InitializeRankedPropertiesRankings(rockets);

        RankProperties();
        AssignRankedPropertiesToRockets();
    }

    private async Task<List<RocketConfigDetailResponse>> GetAllRocketsDetailAsync()
    {
        List<RocketConfigDetailResponse> rockets = new();

        const int maxItemsPerRequest = 100;
        var result = await _launchApi.GetRocketsDetailAsync(maxItemsPerRequest, 0);
        rockets.AddRange(result.Rockets);

        int additionalRequestsRequired = result.Count / maxItemsPerRequest;
        List<Task<RocketsDetailResponse>> tasks = new();
        for (int i = 1; i <= additionalRequestsRequired; ++i)
        {
            tasks.Add(_launchApi.GetRocketsDetailAsync(maxItemsPerRequest, i * maxItemsPerRequest));
        }
        await Task.WhenAll(tasks);

        foreach (var task in tasks)
        {
            rockets.AddRange(task.Result.Rockets);
        }

        return rockets;
    }

    private static Dictionary<RocketRankedPropertyType, List<RocketRankedProperty>> InitializeRankedPropertiesRankings(List<RocketConfigDetailResponse> rockets)
    {
        Dictionary<RocketRankedPropertyType, List<RocketRankedProperty>> propertiesByType = new();
        foreach (var propertyType in Enum.GetValues<RocketRankedPropertyType>())
        {
            propertiesByType.Add(propertyType, new());
        }

        foreach (var rocketResponse in rockets)
        {
            var rocket = rocketResponse.ToModel();

            propertiesByType[RocketRankedPropertyType.Length].Add(new() { Rocket = rocket, Type = RocketRankedPropertyType.Length, 
                Value = rocket.Details.Length });
            propertiesByType[RocketRankedPropertyType.Diameter].Add(new() { Rocket = rocket, Type = RocketRankedPropertyType.Diameter, 
                Value = rocket.Details.Diameter });
            propertiesByType[RocketRankedPropertyType.LaunchCost].Add(new() { Rocket = rocket, Type = RocketRankedPropertyType.LaunchCost, 
                Value = rocket.Details.LaunchCost });
            propertiesByType[RocketRankedPropertyType.LiftoffMass].Add(new() { Rocket = rocket, Type = RocketRankedPropertyType.LiftoffMass, 
                Value = rocket.Details.LiftoffMass });
            propertiesByType[RocketRankedPropertyType.LiftoffThrust].Add(new() { Rocket = rocket, Type = RocketRankedPropertyType.LiftoffThrust, 
                Value = rocket.Details.LiftoffThrust });
            propertiesByType[RocketRankedPropertyType.LeoCapacity].Add(new() { Rocket = rocket, Type = RocketRankedPropertyType.LeoCapacity, 
                Value = rocket.Details.LeoCapacity });
            propertiesByType[RocketRankedPropertyType.GeoCapacity].Add(new() { Rocket = rocket, Type = RocketRankedPropertyType.GeoCapacity, 
                Value = rocket.Details.GeoCapacity });
            propertiesByType[RocketRankedPropertyType.CostPerKgToLeo].Add(new() { Rocket = rocket, Type = RocketRankedPropertyType.CostPerKgToLeo, 
                Value = rocket.Details.CostPerKgToLeo });
            propertiesByType[RocketRankedPropertyType.CostPerKgToGeo].Add(new() { Rocket = rocket, Type = RocketRankedPropertyType.CostPerKgToGeo, 
                Value = rocket.Details.CostPerKgToGeo });
            propertiesByType[RocketRankedPropertyType.SuccessfulLaunches].Add(new() { Rocket = rocket, Type = RocketRankedPropertyType.SuccessfulLaunches, 
                Value = rocket.Details.SuccessfulLaunches });
            propertiesByType[RocketRankedPropertyType.TotalLaunches].Add(new() { Rocket = rocket, Type = RocketRankedPropertyType.TotalLaunches, 
                Value = rocket.Details.TotalLaunchCount });
            propertiesByType[RocketRankedPropertyType.LaunchSuccessPercent].Add(new() { Rocket = rocket, Type = RocketRankedPropertyType.LaunchSuccessPercent, 
                Value = rocket.Details.LaunchSuccessPercent, SecondaryValue = rocket.Details.SuccessfulLaunches });
        }

        return propertiesByType;
    }

    private void RankProperties()
    {
        AscendingComparer<int> ascendingIntComparer = new();
        DescendingComparer<double> descendingDoubleComparer = new();
        DescendingComparer<int> descendingIntComparer = new();

        _rankedPropertiesRankings[RocketRankedPropertyType.Length].Sort(descendingDoubleComparer);
        _rankedPropertiesRankings[RocketRankedPropertyType.Diameter].Sort(descendingDoubleComparer);
        _rankedPropertiesRankings[RocketRankedPropertyType.LaunchCost].Sort(ascendingIntComparer);
        _rankedPropertiesRankings[RocketRankedPropertyType.LiftoffMass].Sort(descendingIntComparer);
        _rankedPropertiesRankings[RocketRankedPropertyType.LiftoffThrust].Sort(descendingIntComparer);
        _rankedPropertiesRankings[RocketRankedPropertyType.LeoCapacity].Sort(descendingIntComparer);
        _rankedPropertiesRankings[RocketRankedPropertyType.GeoCapacity].Sort(descendingIntComparer);
        _rankedPropertiesRankings[RocketRankedPropertyType.CostPerKgToLeo].Sort(ascendingIntComparer);
        _rankedPropertiesRankings[RocketRankedPropertyType.CostPerKgToGeo].Sort(ascendingIntComparer);
        _rankedPropertiesRankings[RocketRankedPropertyType.SuccessfulLaunches].Sort(descendingIntComparer);
        _rankedPropertiesRankings[RocketRankedPropertyType.TotalLaunches].Sort(descendingIntComparer);
        _rankedPropertiesRankings[RocketRankedPropertyType.LaunchSuccessPercent].Sort(descendingIntComparer);
    }

    private void AssignRankedPropertiesToRockets()
    {
        foreach (var (type, properties) in _rankedPropertiesRankings)
        {
            for (int i = 0; i < properties.Count; ++i)
            {
                if (!_rocketRankedProperties.ContainsKey(properties[i].Rocket.ApiId))
                {
                    _rocketRankedProperties[properties[i].Rocket.ApiId] = new();
                }

                _rocketRankedProperties[properties[i].Rocket.ApiId].Add(type, properties[i].Value != null ? i + 1 : null);
            }
        }
    }

    private class AscendingComparer<T> : IComparer<RocketRankedProperty> where T : IComparable<T>
    {
        public int Compare(RocketRankedProperty x, RocketRankedProperty y)
        {
            return ComparerHelper.Compare<T>(x, y, false);
        }
    }

    private class DescendingComparer<T> : IComparer<RocketRankedProperty> where T : IComparable<T>
    {
        public int Compare(RocketRankedProperty x, RocketRankedProperty y)
        {
            return ComparerHelper.Compare<T>(x, y, true);
        }
    }

    private static class ComparerHelper
    {
        public static int Compare<T>(RocketRankedProperty x, RocketRankedProperty y, bool reverseResult) where T : IComparable<T>
        {
            if (x.Value != null && y.Value == null)
            {
                return -1;
            }
            else if (x.Value == null && y.Value != null)
            {
                return 1;
            }
            else if (x.Value == null && y.Value == null)
            {
                return 0;
            }
            else
            {
                var xValue = (T)x.Value;
                var yValue = (T)y.Value;
                var result = xValue.CompareTo(yValue);
                if (result == 0 && x.SecondaryValue != null && y.SecondaryValue != null)
                {
                    var xSecondaryValue = (T)x.SecondaryValue;
                    var ySecondaryValue = (T)y.SecondaryValue;
                    result = xSecondaryValue.CompareTo(ySecondaryValue);
                    return reverseResult ? -result : result;
                }
                else
                {
                    return reverseResult ? -result : result;
                }
            }
        }
    }
}
