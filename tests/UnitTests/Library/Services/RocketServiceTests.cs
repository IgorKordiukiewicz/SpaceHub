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

namespace UnitTests.Library.Services
{
    public class RocketServiceTests
    {
        private readonly RocketService _rocketService;
        private readonly Mock<ILaunchApi> _launchApi = new();
        private readonly Fixture _fixture = new();

        public RocketServiceTests()
        {
            _rocketService = new RocketService(_launchApi.Object);
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
    }
}
