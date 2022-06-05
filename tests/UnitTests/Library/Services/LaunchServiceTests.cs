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
            List<LaunchResponse> expectedResponse = new()
            {
                _fixture.Create<LaunchResponse>()
            };

            List<Launch> expected = expectedResponse.Select(l => l.ToModel()).ToList();

            var launchesResponse = _fixture.Build<LaunchesResponse>().With(l => l.Launches, expectedResponse).Create();  
            _launchApi.Setup(l => l.GetUpcomingLaunchesAsync()).Returns(Task.FromResult(launchesResponse));

            var result = await _launchService.GetUpcomingLaunchesAsync();

            result.Should().BeEquivalentTo(expected, options => options.ComparingByValue<List<Program>>());
        }

        [Fact]
        public async Task GetPreviousLaunchesAsync_ShouldReturnLaunchesList()
        {
            List<LaunchResponse> expectedResponse = new()
            {
                _fixture.Create<LaunchResponse>()
            };

            List<Launch> expected = expectedResponse.Select(l => l.ToModel()).ToList();


            var launchesResponse = _fixture.Build<LaunchesResponse>().With(l => l.Launches, expectedResponse).Create();
            _launchApi.Setup(l => l.GetPreviousLaunchesAsync()).Returns(Task.FromResult(launchesResponse));

            var result = await _launchService.GetPreviousLaunchesAsync();

            result.Should().BeEquivalentTo(expected, options => options.ComparingByValue<List<Program>>());
        }

        [Fact]
        public async Task GetLaunchAsync_ShouldReturnLaunch()
        {
            var expectedResponse = _fixture.Create<LaunchDetailResponse>();
            var expected = expectedResponse.ToModel();
            string launchId = "test";
            _launchApi.Setup(l => l.GetLaunchAsync(launchId)).Returns(Task.FromResult(expectedResponse));

            var result = await _launchService.GetLaunchAsync(launchId);

            //result.Should().Be(expected);
            result.Should().BeEquivalentTo(expected, options => options.ComparingByValue<List<Program>>());
        }
    }
}
