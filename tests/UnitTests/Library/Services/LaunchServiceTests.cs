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

namespace UnitTests.Library.Services
{
    public class LaunchServiceTests
    {
        private readonly LaunchService _launchService;
        private readonly Mock<ILaunchApi> _launchApi = new();
        private readonly Fixture _fixture = new();

        public LaunchServiceTests()
        {
            _launchService = new LaunchService(_launchApi.Object);
        }

        [Fact]
        public async Task GetUpcomingLaunchesAsync_ShouldReturnLaunchesList()
        { 
            List<LaunchResponse> expected = new()
            {
                _fixture.Create<LaunchResponse>()
            };

            var launchesResponse = _fixture.Build<LaunchesResponse>().With(l => l.Launches, expected).Create();  
            _launchApi.Setup(l => l.GetUpcomingLaunchesAsync()).Returns(Task.FromResult(launchesResponse));

            var result = await _launchService.GetUpcomingLaunchesAsync();

            result.Should().Equal(expected);
        }

        [Fact]
        public async Task GetPreviousLaunchesAsync_ShouldReturnLaunchesList()
        {
            List<LaunchResponse> expected = new()
            {
                _fixture.Create<LaunchResponse>()
            };

            var launchesResponse = _fixture.Build<LaunchesResponse>().With(l => l.Launches, expected).Create();
            _launchApi.Setup(l => l.GetPreviousLaunchesAsync()).Returns(Task.FromResult(launchesResponse));

            var result = await _launchService.GetPreviousLaunchesAsync();

            result.Should().Equal(expected);
        }

        [Fact]
        public async Task GetLaunchAsync_ShouldReturnLaunch()
        {
            var expected = _fixture.Create<LaunchDetailResponse>();
            string launchId = "test";
            _launchApi.Setup(l => l.GetLaunchAsync(launchId)).Returns(Task.FromResult(expected));

            var result = await _launchService.GetLaunchAsync(launchId);

            result.Should().Be(expected);
        }
    }
}
