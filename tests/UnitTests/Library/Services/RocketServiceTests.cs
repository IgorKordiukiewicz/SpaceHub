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
            List<RocketConfigResponse> expected = new()
            {
                _fixture.Create<RocketConfigResponse>()
            };
            
            var rocketsResponse = _fixture.Build<RocketsResponse>().With(r => r.Rockets, expected).Create();
            _launchApi.Setup(l => l.GetRocketsAsync(50, 0)).Returns(Task.FromResult(rocketsResponse));
            
            var (itemsCount, result) = await _rocketService.GetRocketsAsync(1); 

            result.Should().Equal(expected);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 50)]
        public async Task GetRocketsAsync_ShouldReturnDifferentRockets_DependingOnPageNumber(int pageNumber, int offset)
        {
            List<RocketConfigResponse> expected = new()
            {
                _fixture.Create<RocketConfigResponse>()
            };

            var rocketsResponse = _fixture.Build<RocketsResponse>().With(r => r.Rockets, expected).Create();
            _launchApi.Setup(l => l.GetRocketsAsync(50, offset)).Returns(Task.FromResult(rocketsResponse));

            var (itemsCount, result) = await _rocketService.GetRocketsAsync(pageNumber);

            result.Should().Equal(expected);
        }
    }
}
