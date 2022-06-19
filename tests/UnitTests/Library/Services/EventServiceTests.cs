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
using FluentAssertions.Execution;

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
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetEventsAsync_ShouldReturnDifferentEvents_DependingOnPageNumber(bool upcoming)
        {
            List<EventResponse> eventResponses1 = new() { _fixture.Create<EventResponse>() };
            List<EventResponse> eventResponses2 = new() { _fixture.Create<EventResponse>() };

            List<Event> expected1 = eventResponses1.Select(a => a.ToModel()).ToList();
            List<Event> expected2 = eventResponses2.Select(a => a.ToModel()).ToList();

            string searchValue = "search";
            var expectedResponse1 = _fixture.Build<EventsResponse>().With(a => a.Events, eventResponses1).Create();
            var expectedResponse2 = _fixture.Build<EventsResponse>().With(a => a.Events, eventResponses2).Create();

            int itemsPerPage = 12;
            if(upcoming)
            {
                _launchApi.Setup(l => l.GetUpcomingEventsAsync(searchValue, itemsPerPage, 0)).Returns(Task.FromResult(expectedResponse1));
                _launchApi.Setup(l => l.GetUpcomingEventsAsync(searchValue, itemsPerPage, itemsPerPage)).Returns(Task.FromResult(expectedResponse2));
            }
            else
            {
                _launchApi.Setup(l => l.GetPreviousEventsAsync(searchValue, itemsPerPage, 0)).Returns(Task.FromResult(expectedResponse1));
                _launchApi.Setup(l => l.GetPreviousEventsAsync(searchValue, itemsPerPage, itemsPerPage)).Returns(Task.FromResult(expectedResponse2));
            }

            var (itemsCount1, result1) = upcoming ? await _eventService.GetUpcomingEventsAsync(searchValue, 1, itemsPerPage)
                : await _eventService.GetPreviousEventsAsync(searchValue, 1, itemsPerPage);
            var (itemsCount2, result2) = upcoming ? await _eventService.GetUpcomingEventsAsync(searchValue, 2, itemsPerPage)
                : await _eventService.GetPreviousEventsAsync(searchValue, 2, itemsPerPage);

            using (new AssertionScope())
            {
                result1.Should().BeEquivalentTo(expected1, options => options.ComparingByValue<List<SpaceProgram>>().ComparingByValue<List<Launch>>());
                result2.Should().BeEquivalentTo(expected2, options => options.ComparingByValue<List<SpaceProgram>>().ComparingByValue<List<Launch>>());

                result1.Should().NotBeEquivalentTo(result2, options => options.ComparingByValue<List<SpaceProgram>>().ComparingByValue<List<Launch>>());
            }
        }

        [Fact]
        public async Task GetEventAsync_ShouldReturnEvent()
        {
            var expectedResponse = _fixture.Create<EventResponse>();
            var expected = expectedResponse.ToModel();

            int eventId = 1;
            _launchApi.Setup(l => l.GetEventAsync(eventId)).Returns(Task.FromResult(expectedResponse));

            var result = await _eventService.GetEventAsync(eventId);

            result.Should().BeEquivalentTo(expected, options => options.ComparingByValue<List<Launch>>().ComparingByValue<List<SpaceProgram>>());
        }
    }
}
