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
using Library.Models;
using Library.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;

namespace UnitTests.Library.Services
{
    public class LaunchServiceTests
    {
        private readonly LaunchService _launchService;
        private readonly Mock<ILaunchApi> _launchApi = new();
        private readonly Fixture _fixture = new();

        public LaunchServiceTests()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();

            _launchService = new LaunchService(_launchApi.Object, new RocketService(_launchApi.Object, memoryCache), memoryCache);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 12)]
        public async Task GetUpcomingLaunchesAsync_ShouldReturnDifferentLaunches_DependingOnPageNumber(int pageNumber, int offset)
        { 
            List<LaunchResponse> expectedResponse = new()
            {
                _fixture.Create<LaunchResponse>()
            };

            List<Launch> expected = expectedResponse.Select(l => l.ToModel()).ToList();

            string searchValue = "search";
            var launchesResponse = _fixture.Build<LaunchesResponse>().With(l => l.Launches, expectedResponse).Create();  
            _launchApi.Setup(l => l.GetUpcomingLaunchesAsync(searchValue, 12, offset)).Returns(Task.FromResult(launchesResponse));

            var (itemsCount, result) = await _launchService.GetUpcomingLaunchesAsync(searchValue, pageNumber);

            result.Should().BeEquivalentTo(expected, options => options.ComparingByValue<List<SpaceProgram>>());
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 12)]
        public async Task GetPreviousLaunchesAsync_ShouldReturnDifferentLaunches_DependingOnPageNumber(int pageNumber, int offset)
        {
            List<LaunchResponse> expectedResponse = new()
            {
                _fixture.Create<LaunchResponse>()
            };

            List<Launch> expected = expectedResponse.Select(l => l.ToModel()).ToList();

            string searchValue = "search";
            var launchesResponse = _fixture.Build<LaunchesResponse>().With(l => l.Launches, expectedResponse).Create();
            _launchApi.Setup(l => l.GetPreviousLaunchesAsync(searchValue, 12, offset)).Returns(Task.FromResult(launchesResponse));

            var (itemsCount, result) = await _launchService.GetPreviousLaunchesAsync(searchValue, pageNumber);

            result.Should().BeEquivalentTo(expected, options => options.ComparingByValue<List<SpaceProgram>>());
        }

        [Fact]
        public async Task GetLaunchAsync_ShouldReturnLaunch()
        {
            var rocketResponse = _fixture.Build<RocketConfigDetailResponse>().With(c => c.LaunchCost, "1000").Create();
            var expectedResponse = _fixture.Build<LaunchDetailResponse>()
                .With(l => l.Rocket, _fixture.Build<RocketDetailResponse>()
                .With(r => r.Configuration, rocketResponse).Create()).Create();
            var expected = expectedResponse.ToModel();

            string launchId = "test";
            _launchApi.Setup(l => l.GetLaunchAsync(launchId)).Returns(Task.FromResult(expectedResponse));
            _launchApi.Setup(l => l.GetRocketsDetailAsync(100, 0)).Returns(Task.FromResult(new RocketsDetailResponse()
            {
                Count = 1,
                Rockets = new List<RocketConfigDetailResponse>()
                {
                    rocketResponse
                }
            }));

            var result = await _launchService.GetLaunchAsync(launchId);

            result.Should().BeEquivalentTo(expected, options => options.ComparingByValue<List<SpaceProgram>>().Excluding(r => r.Rocket.Details.RankedProperties));
        }
    }
}
