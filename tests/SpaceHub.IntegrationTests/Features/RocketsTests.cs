using Bogus;
using FluentAssertions;
using FluentAssertions.Execution;
using SpaceHub.Application.Features.Rockets;
using SpaceHub.Contracts.Models;
using SpaceHub.Infrastructure.Data.Models;
using Xunit;

namespace SpaceHub.IntegrationTests.Features;

[Collection(nameof(IntegrationTestsCollection))]
public class RocketsTests
{
    private readonly IntegrationTestsFixture _fixture;

    public RocketsTests(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;

        _fixture.ResetDb();
    }

    [Fact]
    public async Task GetRockets_ShouldReturnRocketsOrderedByName_WhenSearchValueIsEmpty()
    {
        _fixture.SeedDb(db =>
        {
            int id = 0;
            db.Rockets.InsertMany(new Faker<RocketModel>()
                .RuleFor(x => x.ApiId, f => id++)
                .RuleFor(x => x.Name, f => f.JoinedWords())
                .Generate(15));
        });

        var rockets = await _fixture.GetAsync<RocketModel>();
        var pagination = new Pagination();
        var result = await _fixture.SendRequest(new GetRocketsQuery(string.Empty, pagination));

        using(new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Value.TotalPagesCount.Should().Be(pagination.GetPagesCount(rockets.Count));
            result.Value.Rockets.Count.Should().Be(pagination.ItemsPerPage);

            var expectedRockets = rockets.OrderBy(x => x.Name).Select(x => x.ApiId).Take(result.Value.Rockets.Count).ToList();
            var actualRockets = result.Value.Rockets.Select(x => x.ApiId).ToList();
            actualRockets.Should().BeEquivalentTo(expectedRockets, options => options.WithStrictOrdering());
        }
    }

    [Fact]
    public async Task GetRockets_ShouldReturnRocketsFilteredBySearchValue_WhenSearchValueIsProvided()
    {
        _fixture.SeedDb(db =>
        {
            int id = 0;
            void InsertRocket(string name)
            {
                db.Rockets.InsertMany(new Faker<RocketModel>()
                    .RuleFor(x => x.ApiId, f => id++)
                    .RuleFor(x => x.Name, f => name)
                    .Generate(1));
            }
            InsertRocket("Foo");
            InsertRocket("Bar");
        });

        var result = await _fixture.SendRequest(new GetRocketsQuery("fo", new()));

        using(new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Value.TotalPagesCount.Should().Be(1);
            result.Value.Rockets.Count.Should().Be(1);
            result.Value.Rockets[0].Name.Should().Be("Foo");
        }
    }
}
