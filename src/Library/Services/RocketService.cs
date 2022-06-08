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

namespace Library.Services
{
    public class RocketService : IRocketService
    {
        private readonly ILaunchApi _launchApi;
        private readonly Pagination _pagination = new() { ItemsPerPage = 50 };
        private Dictionary<int, Dictionary<RocketRankedPropertyType, int?>>? _rocketRankedProperties;

        public RocketService(ILaunchApi launchApi)
        {
            _launchApi = launchApi;
        }

        public async Task<(int, List<Rocket>)> GetRocketsAsync(string? searchValue, int pageNumber)
        {
            var offset = _pagination.GetOffset(pageNumber);
            var result = await _launchApi.GetRocketsAsync(searchValue, _pagination.ItemsPerPage, offset);
            var pagesCount = _pagination.GetPagesCount(result.Count);

            return (pagesCount, result.Rockets.Select(r => r.ToModel()).ToList());
        }

        public async Task<Rocket> GetRocketAsync(int id)
        {
            var result = await _launchApi.GetRocketAsync(id);

            return result.ToModel();
        }

        public async Task<Dictionary<RocketRankedPropertyType, int?>?> GetRocketRankedProperties(int id)
        {
            if(_rocketRankedProperties == null)
            {
                await InitializeRocketRankedPropertiesAsync();
            }

            return _rocketRankedProperties.GetValueOrDefault(id);
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
            for (int i = 1; i <= additionalRequestsRequired; ++i)
            {
                result = await _launchApi.GetRocketsDetailAsync(maxItemsPerRequest, i * maxItemsPerRequest);
                rockets.AddRange(result.Rockets);
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
        }

        private class AscendingComparer<T> : IComparer<RocketRankedProperty> where T : IComparable<T>
        {
            public int Compare(RocketRankedProperty x, RocketRankedProperty y)
            {
                int? result = ComparerHelper.CompareNulls(x, y);
                if (result == null)
                {
                    var xValue = (T)x.Value;
                    var yValue = (T)y.Value;
                    return xValue.CompareTo(yValue);
                }
                else
                {
                    return result.Value;
                }
            }
        }

        private class DescendingComparer<T> : IComparer<RocketRankedProperty> where T : IComparable<T>
        {
            public int Compare(RocketRankedProperty x, RocketRankedProperty y)
            {
                int? result = ComparerHelper.CompareNulls(x, y);
                if (result == null)
                {
                    var xValue = (T)x.Value;
                    var yValue = (T)y.Value;
                    return -xValue.CompareTo(yValue);
                }
                else
                {
                    return result.Value;
                }
            }
        }

        private static class ComparerHelper
        {
            public static int? CompareNulls(RocketRankedProperty x, RocketRankedProperty y)
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
                    return null;
                }
            }
        }
    }
}
