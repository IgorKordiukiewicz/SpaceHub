﻿using Bogus;
using FluentAssertions;
using FluentAssertions.Execution;
using SpaceHub.Application.Common;
using SpaceHub.Application.Errors;
using SpaceHub.Application.Features.Launches;
using SpaceHub.Contracts.Enums;
using SpaceHub.Contracts.Models;
using SpaceHub.Domain.Models;
using SpaceHub.Infrastructure.Data;
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

        _fixture.ResetDb();
    }

    [Fact]
    public async Task GetLaunches_ShouldReturnUpcomingLaunchesOrderedByDate_WhenTimeFrameIsUpcomingAndSearchValueIsEmpty()
    {
        _fixture.SeedDb(SeedGetLaunchesWithoutSearchValueData);

        var launches = await _fixture.GetAsync<Launch>(x => x.Date > _dateTimeProvider.Now());
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
        _fixture.SeedDb(SeedGetLaunchesWithoutSearchValueData);

        var launches = await _fixture.GetAsync<Launch>(x => x.Date <= _dateTimeProvider.Now());
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
        _fixture.SeedDb(db =>
        {
            void InsertLaunch(string name)
            {
                db.Launches.InsertMany(new Faker<Launch>()
                .RuleFor(x => x.ApiId, f => f.Random.Guid())
                .RuleFor(x => x.Name, f => name)
                .RuleFor(x => x.Date, f => f.Date.Future(1, _dateTimeProvider.Now()))
                .RuleFor(x => x.Pad, f => new Faker<LaunchPad>().Generate())
                .Generate(1));
            }
            InsertLaunch("Foo");
            InsertLaunch("Bar");
        });

        var result = await _fixture.SendRequest(new GetLaunchesQuery(ETimeFrame.Upcoming, "fo", new()));

        using(new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Value.TotalPagesCount.Should().Be(1);
            result.Value.Launches.Count.Should().Be(1);
            result.Value.Launches[0].Name.Should().Be("Foo");
        }
    }

    [Fact]
    public async Task GetLaunchDetails_ShouldReturnRecordNotFoundError_WhenLaunchIsNotFound()
    {
        _fixture.SeedDb(db =>
        {
            db.Launches.InsertMany(new Faker<Launch>()
                .RuleFor(x => x.ApiId, Guid.NewGuid())
                .Generate(1));
        });

        var result = await _fixture.SendRequest(new GetLaunchDetailsQuery(Guid.NewGuid()));

        using(new AssertionScope())
        {
            result.IsSuccess.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
            result.Errors[0].Should().BeOfType<RecordNotFoundError>();
        }
    }

    [Fact]
    public async Task GetLaunchDetails_ShouldReturnRecordNotFoundError_WhenAgencyIsNotFound()
    {
        var launchId = Guid.NewGuid();
        _fixture.SeedDb(db =>
        {
            db.Launches.InsertMany(new Faker<Launch>()
                .RuleFor(x => x.ApiId, launchId)
                .RuleFor(x => x.AgencyApiId, 0)
                .Generate(1));

            db.Agencies.InsertMany(new Faker<Agency>()
                .RuleFor(x => x.ApiId, 1)
                .Generate(1));
        });

        var result = await _fixture.SendRequest(new GetLaunchDetailsQuery(launchId));

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
            result.Errors[0].Should().BeOfType<RecordNotFoundError>();
        }
    }

    [Fact]
    public async Task GetLaunchDetails_ShouldReturnRecordNotFoundError_WhenRocketIsNotFound()
    {
        var launchId = Guid.NewGuid();
        _fixture.SeedDb(db =>
        {
            db.Launches.InsertMany(new Faker<Launch>()
                .RuleFor(x => x.ApiId, launchId)
                .RuleFor(x => x.AgencyApiId, 1)
                .RuleFor(x => x.RocketApiId, 0)
                .Generate(1));

            db.Agencies.InsertMany(new Faker<Agency>()
                .RuleFor(x => x.ApiId, 1)
                .Generate(1));

            db.Rockets.InsertMany(new Faker<Rocket>()
                .RuleFor(x => x.ApiId, 1)
                .Generate(1));
        });

        var result = await _fixture.SendRequest(new GetLaunchDetailsQuery(launchId));

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
            result.Errors[0].Should().BeOfType<RecordNotFoundError>();
        }
    }

    [Fact]
    public async Task GetLaunchDetails_ShouldReturnSuccess_WhenLaunchAndItsAgencyAndRocketAreFound()
    {
        var launchId = Guid.NewGuid();
        _fixture.SeedDb(db =>
        {
            db.Launches.InsertMany(new Faker<Launch>()
                .RuleFor(x => x.ApiId, launchId)
                .RuleFor(x => x.AgencyApiId, 1)
                .RuleFor(x => x.RocketApiId, 1)
                .Generate(1));

            db.Agencies.InsertMany(new Faker<Agency>()
                .RuleFor(x => x.ApiId, 1)
                .Generate(1));

            db.Rockets.InsertMany(new Faker<Rocket>()
                .RuleFor(x => x.ApiId, 1)
                .Generate(1));
        });

        var result = await _fixture.SendRequest(new GetLaunchDetailsQuery(launchId));

        using (new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Value.Agency.Should().NotBeNull();
            result.Value.Rocket.Should().NotBeNull();
        }
    }

    private static void SeedGetLaunchesWithoutSearchValueData(DbContext db)
    {
        var date = new DateTime(2023, 3, 15);
        void InsertLaunches(bool upcoming)
        {
            db.Launches.InsertMany(new Faker<Launch>()
                .RuleFor(x => x.ApiId, f => f.Random.Guid())
                .RuleFor(x => x.Name, f => f.JoinedWords())
                .RuleFor(x => x.Date, f => upcoming ? f.Date.Future(1, date) : f.Date.Past(1, date))
                .RuleFor(x => x.Pad, f => new Faker<LaunchPad>().Generate())
                .RuleFor(x => x.Videos, f => new Faker<LaunchVideo>().Generate(2))
                .Generate(15));
        }
        InsertLaunches(true);
        InsertLaunches(false);
    }
}
