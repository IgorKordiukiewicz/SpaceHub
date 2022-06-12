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
using FluentAssertions.Execution;

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
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetLaunchesAsync_ShouldReturnDifferentLaunches_DependingOnPageNumber(bool upcoming)
        {
            List<LaunchResponse> launchResponses1 = new() { _fixture.Create<LaunchResponse>() };
            List<LaunchResponse> launchResponses2 = new() { _fixture.Create<LaunchResponse>() };

            List<Launch> expected1 = launchResponses1.Select(l => l.ToModel()).ToList();
            List<Launch> expected2 = launchResponses2.Select(l => l.ToModel()).ToList();

            string searchValue = "search";
            var expectedResponse1 = _fixture.Build<LaunchesResponse>().With(l => l.Launches, launchResponses1).Create();
            var expectedResponse2 = _fixture.Build<LaunchesResponse>().With(l => l.Launches, launchResponses2).Create();

            int itemsPerPage = _launchService.Pagination.ItemsPerPage;
            if(upcoming)
            {
                _launchApi.Setup(l => l.GetUpcomingLaunchesAsync(searchValue, itemsPerPage, 0)).Returns(Task.FromResult(expectedResponse1));
                _launchApi.Setup(l => l.GetUpcomingLaunchesAsync(searchValue, itemsPerPage, itemsPerPage)).Returns(Task.FromResult(expectedResponse2));
            }
            else
            {
                _launchApi.Setup(l => l.GetPreviousLaunchesAsync(searchValue, itemsPerPage, 0)).Returns(Task.FromResult(expectedResponse1));
                _launchApi.Setup(l => l.GetPreviousLaunchesAsync(searchValue, itemsPerPage, itemsPerPage)).Returns(Task.FromResult(expectedResponse2));
            }

            var (itemsCount1, result1) = upcoming ? await _launchService.GetUpcomingLaunchesAsync(searchValue, 1)
                : await _launchService.GetPreviousLaunchesAsync(searchValue, 1);
            var (itemsCount2, result2) = upcoming ? await _launchService.GetUpcomingLaunchesAsync(searchValue, 2)
                : await _launchService.GetPreviousLaunchesAsync(searchValue, 2);

            using (new AssertionScope())
            {
                result1.Should().BeEquivalentTo(expected1, options => options.ComparingByValue<List<SpaceProgram>>());
                result2.Should().BeEquivalentTo(expected2, options => options.ComparingByValue<List<SpaceProgram>>());

                result1.Should().NotBeEquivalentTo(result2, options => options.ComparingByValue<List<SpaceProgram>>());
            }
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
