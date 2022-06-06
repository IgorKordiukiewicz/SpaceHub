using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Moq;
using AutoFixture;
using Library.Services;
using Library.Api;
using Library.Api.Responses;
using System.Linq.Expressions;
using Library.Models;
using Library.Mapping;
using FluentAssertions.Execution;
using Library.Enums;

namespace UnitTests.Library.Services
{
    public class RocketServiceTests
    {
        private readonly RocketService _rocketService;
        private readonly Mock<ILaunchApi> _launchApi = new();
        private readonly Fixture _fixture = new();
        private readonly RocketRankedPropertyTestHelper _rocketRankedPropertyTestHelper;

        public RocketServiceTests()
        {
            _rocketService = new(_launchApi.Object);
            _rocketRankedPropertyTestHelper = new(_rocketService, _launchApi, _fixture);
        }

        [Fact]
        public async Task GetRocketsAsync_ShouldReturnRocketsList()
        {
            List<RocketConfigResponse> expectedResponse = new()
            {
                _fixture.Create<RocketConfigResponse>()
            };

            List<Rocket> expected = expectedResponse.Select(r => r.ToModel()).ToList();
            
            var rocketsResponse = _fixture.Build<RocketsResponse>().With(r => r.Rockets, expectedResponse).Create();
            _launchApi.Setup(l => l.GetRocketsAsync(50, 0)).Returns(Task.FromResult(rocketsResponse));
            
            var (itemsCount, result) = await _rocketService.GetRocketsAsync(1); 

            result.Should().Equal(expected);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 50)]
        public async Task GetRocketsAsync_ShouldReturnDifferentRockets_DependingOnPageNumber(int pageNumber, int offset)
        {
            List<RocketConfigResponse> expectedResponse = new()
            {
                _fixture.Create<RocketConfigResponse>()
            };

            List<Rocket> expected = expectedResponse.Select(r => r.ToModel()).ToList();

            var rocketsResponse = _fixture.Build<RocketsResponse>().With(r => r.Rockets, expectedResponse).Create();
            _launchApi.Setup(l => l.GetRocketsAsync(50, offset)).Returns(Task.FromResult(rocketsResponse));

            var (itemsCount, result) = await _rocketService.GetRocketsAsync(pageNumber);

            result.Should().Equal(expected);
        }

        [Fact]
        public async Task GetRocketAsync_ShouldReturnRocket()
        {
            var expectedResponse = _fixture.Build<RocketConfigDetailResponse>().With(r => r.LaunchCost, "1000").Create();
            var expected = expectedResponse.ToModel();
            int id = 1;
            _launchApi.Setup(l => l.GetRocketAsync(id)).Returns(Task.FromResult(expectedResponse));

            var result = await _rocketService.GetRocketAsync(id);

            result.Should().Be(expected);
        }

        [Fact]
        public async Task GetRocketRankedProperties_ShouldReturnNull_WhenIdIsNotFound()
        {
            RocketsDetailResponse expectedResponse = new()
            {
                Count = 1,
                Rockets = new List<RocketConfigDetailResponse>()
                {
                    _fixture.Build<RocketConfigDetailResponse>().With(r => r.Id, 1).Create()
                }
            };
            _launchApi.Setup(l => l.GetRocketsDetailAsync(100, 0)).Returns(Task.FromResult(expectedResponse));

            var result = await _rocketService.GetRocketRankedProperties(2);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetRocketRankedProperties_LengthShouldBeRankedProperly()
        {
            var result = await _rocketRankedPropertyTestHelper.RunSingleProp(RocketRankedPropertyType.Length, r => r.Length, 2.0, null, 3.0);

            result.Should().Equal(new int[3] { 2, 3, 1 });
        }

        [Fact]
        public async Task GetRocketRankedProperties_DiameterShouldBeRankedProperly()
        {
            var result = await _rocketRankedPropertyTestHelper.RunSingleProp(RocketRankedPropertyType.Diameter, r => r.Diameter, 2.0, null, 3.0);

            result.Should().Equal(new int[3] { 2, 3, 1 });
        }

        [Fact]
        public async Task GetRocketRankedProperties_LaunchCostShouldBeRankedProperly()
        {
            var result = await _rocketRankedPropertyTestHelper.RunSingleProp(RocketRankedPropertyType.LaunchCost, r => r.LaunchCost, "100", null, "200");

            result.Should().Equal(new int[3] { 1, 3, 2 });
        }

        [Fact]
        public async Task GetRocketRankedProperties_LiftoffMassShouldBeRankedProperly()
        {
            var result = await _rocketRankedPropertyTestHelper.RunSingleProp(RocketRankedPropertyType.LiftoffMass, r => r.LaunchMass, 10, null, 20);

            result.Should().Equal(new int[3] { 2, 3, 1 });
        }

        [Fact]
        public async Task GetRocketRankedProperties_LiftoffThrustShouldBeRankedProperly()
        {
            var result = await _rocketRankedPropertyTestHelper.RunSingleProp(RocketRankedPropertyType.LiftoffThrust, r => r.ThrustAtLiftoff, 10, null, 20);

            result.Should().Equal(new int[3] { 2, 3, 1 });
        }

        [Fact]
        public async Task GetRocketRankedProperties_LeoCapacityShouldBeRankedProperly()
        {
            var result = await _rocketRankedPropertyTestHelper.RunSingleProp(RocketRankedPropertyType.LeoCapacity, r => r.LeoCapacity, 10, null, 20);

            result.Should().Equal(new int[3] { 2, 3, 1 });
        }

        [Fact]
        public async Task GetRocketRankedProperties_GeoCapacityShouldBeRankedProperly()
        {
            var result = await _rocketRankedPropertyTestHelper.RunSingleProp(RocketRankedPropertyType.GeoCapacity, r => r.GeoCapacity, 10, null, 20);

            result.Should().Equal(new int[3] { 2, 3, 1 });
        }

        [Fact]
        public async Task GetRocketRankedProperties_CostPerKgToLeoShouldBeRankedProperly()
        {
            var result = await _rocketRankedPropertyTestHelper.RunDoubleProp(RocketRankedPropertyType.CostPerKgToLeo
                , r => r.LaunchCost, "100", null, "200"
                , r => r.LeoCapacity, 10, 10, 10);

            result.Should().Equal(new int[3] { 1, 3, 2 });
        }

        [Fact]
        public async Task GetRocketRankedProperties_CostPerKgToGeoShouldBeRankedProperly()
        {
            var result = await _rocketRankedPropertyTestHelper.RunDoubleProp(RocketRankedPropertyType.CostPerKgToGeo
                , r => r.LaunchCost, "100", null, "200"
                , r => r.GeoCapacity, 10, 10, 10);

            result.Should().Equal(new int[3] { 1, 3, 2 });
        }

        private class RocketRankedPropertyTestHelper
        {
            private readonly RocketService _rocketService;
            private readonly Mock<ILaunchApi> _launchApi;
            private readonly Fixture _fixture;

            public RocketRankedPropertyTestHelper(RocketService rocketService, Mock<ILaunchApi> launchApi, Fixture fixture)
            {
                _rocketService = rocketService;
                _launchApi = launchApi;
                _fixture = fixture;
            }
            
            public async Task<int[]> RunSingleProp<T>(RocketRankedPropertyType propertyType, Expression<Func<RocketConfigDetailResponse, T?>> propertyPicker
                , T? value1, T? value2, T? value3)
            {
                RocketsDetailResponse expectedResponse = new()
                {
                    Count = 3,
                    Rockets = new List<RocketConfigDetailResponse>()
                    {
                        _fixture.Build<RocketConfigDetailResponse>().With(r => r.Id, 1).With(propertyPicker, value1).Create(),
                        _fixture.Build<RocketConfigDetailResponse>().With(r => r.Id, 2).With(propertyPicker, value2).Create(),
                        _fixture.Build<RocketConfigDetailResponse>().With(r => r.Id, 3).With(propertyPicker, value3).Create(),
                    }
                };
                _launchApi.Setup(l => l.GetRocketsDetailAsync(100, 0)).Returns(Task.FromResult(expectedResponse));

                var rocket1Result = await _rocketService.GetRocketRankedProperties(1);
                var rocket2Result = await _rocketService.GetRocketRankedProperties(2);
                var rocket3Result = await _rocketService.GetRocketRankedProperties(3);

                return new int[3] { rocket1Result[propertyType], rocket2Result[propertyType], rocket3Result[propertyType] };
            }

            public async Task<int[]> RunDoubleProp<TA, TB>(RocketRankedPropertyType propertyType
                , Expression<Func<RocketConfigDetailResponse, TA?>> propertyPickerA, TA? value1A, TA? value2A, TA? value3A
                , Expression<Func<RocketConfigDetailResponse, TB?>> propertyPickerB, TB? value1B, TB? value2B, TB? value3B)
            {
                RocketsDetailResponse expectedResponse = new()
                {
                    Count = 3,
                    Rockets = new List<RocketConfigDetailResponse>()
                    {
                        _fixture.Build<RocketConfigDetailResponse>().With(r => r.Id, 1).With(propertyPickerA, value1A).With(propertyPickerB, value1B).Create(),
                        _fixture.Build<RocketConfigDetailResponse>().With(r => r.Id, 2).With(propertyPickerA, value2A).With(propertyPickerB, value2B).Create(),
                        _fixture.Build<RocketConfigDetailResponse>().With(r => r.Id, 3).With(propertyPickerA, value3A).With(propertyPickerB, value3B).Create(),
                    }
                };
                _launchApi.Setup(l => l.GetRocketsDetailAsync(100, 0)).Returns(Task.FromResult(expectedResponse));

                var rocket1Result = await _rocketService.GetRocketRankedProperties(1);
                var rocket2Result = await _rocketService.GetRocketRankedProperties(2);
                var rocket3Result = await _rocketService.GetRocketRankedProperties(3);

                return new int[3] { rocket1Result[propertyType], rocket2Result[propertyType], rocket3Result[propertyType] };
            }
        }
    }
}
