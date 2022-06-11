using Library.Api;
using Library.Models;
using Library.Mapping;
using Library.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Enums;
using Library.Api.Responses;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Services
{
    public class RocketService : IRocketService
    {
        private readonly ILaunchApi _launchApi;
        private readonly Pagination _pagination = new() { ItemsPerPage = 12 };
        private readonly IMemoryCache _cache;
        private Dictionary<int, Dictionary<RocketRankedPropertyType, int?>>? _rocketRankedProperties;

        public RocketService(ILaunchApi launchApi, IMemoryCache cachce)
        {
            _launchApi = launchApi;
            _cache = cachce;
        }

        public async Task<(int, List<Rocket>)> GetRocketsAsync(string? searchValue, int pageNumber)
        {
            var offset = _pagination.GetOffset(pageNumber);
            var result = await _cache.GetOrCreateAsync("rockets" + searchValue + pageNumber.ToString(), async entry =>
            {
                return await _launchApi.GetRocketsAsync(searchValue, _pagination.ItemsPerPage, offset);
            });
            var pagesCount = _pagination.GetPagesCount(result.Count);

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

        private async Task InitializeRocketRankedPropertiesAsync()
        {
            _rocketRankedProperties = new();

            var rockets = await GetAllRocketsDetailAsync();
            var propertiesByType = InitializeRankedPropertiesByType(rockets);

            RankProperties(propertiesByType);
            AssignRankedPropertiesToRockets(propertiesByType);
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

        private static Dictionary<RocketRankedPropertyType, List<RocketRankedProperty>> InitializeRankedPropertiesByType(List<RocketConfigDetailResponse> rockets)
        {
            Dictionary<RocketRankedPropertyType, List<RocketRankedProperty>> propertiesByType = new();
            foreach (var propertyType in Enum.GetValues<RocketRankedPropertyType>())
            {
                propertiesByType.Add(propertyType, new());
            }

            foreach (var rocketResponse in rockets)
            {
                var rocketDetails = rocketResponse.ToModel().Details;
                var id = rocketResponse.Id;

                propertiesByType[RocketRankedPropertyType.Length].Add(new() { RocketId = id, Value = rocketDetails.Length });
                propertiesByType[RocketRankedPropertyType.Diameter].Add(new() { RocketId = id, Value = rocketDetails.Diameter });
                propertiesByType[RocketRankedPropertyType.LaunchCost].Add(new() { RocketId = id, Value = rocketDetails.LaunchCost });
                propertiesByType[RocketRankedPropertyType.LiftoffMass].Add(new() { RocketId = id, Value = rocketDetails.LiftoffMass });
                propertiesByType[RocketRankedPropertyType.LiftoffThrust].Add(new() { RocketId = id, Value = rocketDetails.LiftoffThrust });
                propertiesByType[RocketRankedPropertyType.LeoCapacity].Add(new() { RocketId = id, Value = rocketDetails.LeoCapacity });
                propertiesByType[RocketRankedPropertyType.GeoCapacity].Add(new() { RocketId = id, Value = rocketDetails.GeoCapacity });
                propertiesByType[RocketRankedPropertyType.CostPerKgToLeo].Add(new() { RocketId = id, Value = rocketDetails.CostPerKgToLeo });
                propertiesByType[RocketRankedPropertyType.CostPerKgToGeo].Add(new() { RocketId = id, Value = rocketDetails.CostPerKgToGeo });
                propertiesByType[RocketRankedPropertyType.SuccessfulLaunches].Add(new() { RocketId = id, Value = rocketDetails.SuccessfulLaunches });
                propertiesByType[RocketRankedPropertyType.TotalLaunches].Add(new() { RocketId = id, Value = rocketDetails.TotalLaunchCount });
                propertiesByType[RocketRankedPropertyType.LaunchSuccessPercent].Add(new() { RocketId = id, Value = rocketDetails.LaunchSuccessPercent, 
                    SecondaryValue = rocketDetails.SuccessfulLaunches });
            }

            return propertiesByType;
        }

        private static void RankProperties(Dictionary<RocketRankedPropertyType, List<RocketRankedProperty>> propertiesByType)
        {
            AscendingComparer<int> ascendingIntComparer = new();
            DescendingComparer<double> descendingDoubleComparer = new();
            DescendingComparer<int> descendingIntComparer = new();

            propertiesByType[RocketRankedPropertyType.Length].Sort(descendingDoubleComparer);
            propertiesByType[RocketRankedPropertyType.Diameter].Sort(descendingDoubleComparer);
            propertiesByType[RocketRankedPropertyType.LaunchCost].Sort(ascendingIntComparer);
            propertiesByType[RocketRankedPropertyType.LiftoffMass].Sort(descendingIntComparer);
            propertiesByType[RocketRankedPropertyType.LiftoffThrust].Sort(descendingIntComparer);
            propertiesByType[RocketRankedPropertyType.LeoCapacity].Sort(descendingIntComparer);
            propertiesByType[RocketRankedPropertyType.GeoCapacity].Sort(descendingIntComparer);
            propertiesByType[RocketRankedPropertyType.CostPerKgToLeo].Sort(ascendingIntComparer);
            propertiesByType[RocketRankedPropertyType.CostPerKgToGeo].Sort(ascendingIntComparer);
            propertiesByType[RocketRankedPropertyType.SuccessfulLaunches].Sort(descendingIntComparer);
            propertiesByType[RocketRankedPropertyType.TotalLaunches].Sort(descendingIntComparer);
            propertiesByType[RocketRankedPropertyType.LaunchSuccessPercent].Sort(descendingIntComparer);
        }

        private void AssignRankedPropertiesToRockets(Dictionary<RocketRankedPropertyType, List<RocketRankedProperty>> propertiesByType)
        {
            foreach (var (type, properties) in propertiesByType)
            {
                for (int i = 0; i < properties.Count; ++i)
                {
                    if (!_rocketRankedProperties.ContainsKey(properties[i].RocketId))
                    {
                        _rocketRankedProperties[properties[i].RocketId] = new();
                    }

                    _rocketRankedProperties[properties[i].RocketId].Add(type, properties[i].Value != null ? i + 1 : null);
                }
            }
        }

        private class RocketRankedProperty
        {
            public int RocketId { get; set; }
            public object? Value { get; set; }
            public object? SecondaryValue { get; set; }
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
}
