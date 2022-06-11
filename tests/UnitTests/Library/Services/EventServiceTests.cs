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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Library.Models;
using Library.Mapping;

namespace UnitTests.Library.Services
{
    public class EventServiceTests
    {
        private readonly EventService _eventService;
        private readonly Mock<ILaunchApi> _launchApi = new();
        private readonly Fixture _fixture = new();

        public EventServiceTests()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();

            _eventService = new(_launchApi.Object, memoryCache);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 12)]
        public async Task GetUpcomingEventsAsync_ShouldReturnDifferentEvents_DependingOnPageNumber(int pageNumber, int offset)
        {
            List<EventResponse> expectedResponse = new()
            {
                _fixture.Create<EventResponse>()
            };

            List<Event> expected = expectedResponse.Select(a => a.ToModel()).ToList();

            string searchValue = "search";
            var eventsResponse = _fixture.Build<EventsResponse>().With(a => a.Events, expectedResponse).Create();
            _launchApi.Setup(l => l.GetUpcomingEventsAsync(searchValue, 12, offset)).Returns(Task.FromResult(eventsResponse));

            var (itemsCount, result) = await _eventService.GetUpcomingEventsAsync(searchValue, pageNumber);

            result.Should().Equal(expected);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 12)]
        public async Task GetPreviousEventsAsync_ShouldReturnDifferentEvents_DependingOnPageNumber(int pageNumber, int offset)
        {
            List<EventResponse> expectedResponse = new()
            {
                _fixture.Create<EventResponse>()
            };

            List<Event> expected = expectedResponse.Select(a => a.ToModel()).ToList();

            string searchValue = "search";
            var eventsResponse = _fixture.Build<EventsResponse>().With(a => a.Events, expectedResponse).Create();
            _launchApi.Setup(l => l.GetPreviousEventsAsync(searchValue, 12, offset)).Returns(Task.FromResult(eventsResponse));

            var (itemsCount, result) = await _eventService.GetPreviousEventsAsync(searchValue, pageNumber);

            result.Should().Equal(expected);
        }
    }
}
