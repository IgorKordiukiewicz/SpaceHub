using FluentAssertions;
using FluentAssertions.Execution;
using SpaceHub.Application.Common;
using SpaceHub.Application.Features.Launches;
using SpaceHub.Contracts.Enums;
using SpaceHub.Contracts.Models;
using SpaceHub.Infrastructure.Data.Models;
using Xunit;

namespace SpaceHub.IntegrationTests.Features;

[Collection(nameof(IntegrationTestsCollection))]
public class LaunchesTests
{
    private readonly IntegrationTestsFixture _fixture;
    private readonly TestDateTimeProvider _dateTimeProvider = new();

    public LaunchesTests(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;

        _fixture.InitDb();
    }

    [Fact]
    public async Task GetLaunches_ShouldReturnUpcomingLaunchesOrderedByDate_WhenTimeFrameIsUpcomingAndSearchValueIsEmpty()
    {
        var launches = await _fixture.GetAsync<LaunchModel>(x => x.Date > _dateTimeProvider.Now());
        var pagination = new Pagination();
        var result = await _fixture.SendRequest(new GetLaunchesQuery(ETimeFrame.Upcoming, string.Empty, pagination));

        using(new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Value.TotalPagesCount.Should().Be(pagination.GetPagesCount(launches.Count));
            result.Value.Launches.Count.Should().Be(pagination.ItemsPerPage);

            var expectedLaunches = launches.OrderBy(x => x.Date).Select(x => x.ApiId).Take(result.Value.Launches.Count).ToList();
            var actualLaunches = result.Value.Launches.Select(x => x.Id).ToList();
            actualLaunches.Should().BeEquivalentTo(expectedLaunches, options => options.WithStrictOrdering());
        }
    }

    [Fact]
    public async Task GetLaunches_ShouldReturnPreviousLaunchesOrderedByDateDescending_WhenTimeFrameIsPreviousAndSearchValueIsEmpty()
    {
        var launches = await _fixture.GetAsync<LaunchModel>(x => x.Date <= _dateTimeProvider.Now());
        var pagination = new Pagination();
        var result = await _fixture.SendRequest(new GetLaunchesQuery(ETimeFrame.Previous, string.Empty, pagination));

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Value.TotalPagesCount.Should().Be(pagination.GetPagesCount(launches.Count));
            result.Value.Launches.Count.Should().Be(pagination.ItemsPerPage);

            var expectedLaunches = launches.OrderByDescending(x => x.Date).Select(x => x.ApiId).Take(result.Value.Launches.Count).ToList();
            var actualLaunches = result.Value.Launches.Select(x => x.Id).ToList();
            actualLaunches.Should().BeEquivalentTo(expectedLaunches, options => options.WithStrictOrdering());
        }
    }

    [Fact]
    public async Task GetLaunches_ShouldReturnLaunchesFilteredBySearchValue_WhenSearchValueIsProvided()
    {
        var launch = await _fixture.FirstAsync<LaunchModel>();
        var timeFrame = launch.Date > _dateTimeProvider.Now() ? ETimeFrame.Upcoming : ETimeFrame.Previous;
        var result = await _fixture.SendRequest(new GetLaunchesQuery(timeFrame, launch.Name, new()));

        using(new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Value.TotalPagesCount.Should().Be(1);
            result.Value.Launches.Count.Should().Be(1);
            result.Value.Launches[0].Name.Should().Be(launch.Name);
        }
    }
}
