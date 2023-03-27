using Bogus;
using FluentAssertions;
using FluentAssertions.Execution;
using SpaceHub.Application.Features.News;
using SpaceHub.Contracts.Models;
using SpaceHub.Infrastructure.Data.Models;
using Xunit;

namespace SpaceHub.IntegrationTests.Features;

[Collection(nameof(IntegrationTestsCollection))]
public class NewsTests
{
    private readonly IntegrationTestsFixture _fixture;

    public NewsTests(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;

        _fixture.ResetDb();
    }

    [Fact]
    public async Task GetNews_ShouldReturnArticlesOrderedByDate_WhenSearchValueIsEmpty()
    {
        _fixture.SeedDb(db =>
        {
            db.Articles.InsertMany(new Faker<ArticleModel>()
                .RuleFor(x => x.PublishDate, f => f.Date.Recent())
                .RuleFor(x => x.Title, f => f.JoinedWords())
                .RuleFor(x => x.Summary, f => f.Lorem.Word())
                .Generate(15));
        });

        var articles = await _fixture.GetAsync<ArticleModel>();
        var pagination = new Pagination();
        var result = await _fixture.SendRequest(new GetNewsQuery(string.Empty, pagination));

        using(new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Value.TotalPagesCount.Should().Be(pagination.GetPagesCount(articles.Count));
            result.Value.Articles.Count.Should().Be(pagination.ItemsPerPage);

            var expectedArticles = articles.OrderByDescending(x => x.PublishDate).Select(x => x.Title).Take(result.Value.Articles.Count).ToList();
            var actualArticles = result.Value.Articles.OrderByDescending(x => x.PublishDate).Select(x => x.Title).ToList();
            actualArticles.Should().BeEquivalentTo(expectedArticles, options => options.WithStrictOrdering());
        }
    }

    [Fact]
    public async Task GetNews_ShouldReturnArticlesMatchingSearchCriteria_WhenSearchValueIsProvided()
    {
        _fixture.SeedDb(db =>
        {
            void InsertArticle(string title)
            {
                db.Articles.InsertMany(new Faker<ArticleModel>()
                .RuleFor(x => x.PublishDate, f => f.Date.Recent())
                .RuleFor(x => x.Title, f => title)
                .RuleFor(x => x.Summary, f => string.Empty)
                .Generate(1));
            }
            InsertArticle("Foo");
            InsertArticle("Bar");
        });

        var result = await _fixture.SendRequest(new GetNewsQuery("fo", new()));

        using(new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Value.TotalPagesCount.Should().Be(1);
            result.Value.Articles.Count.Should().Be(1);
            result.Value.Articles[0].Title.Should().Be("Foo");
        }
    }
}
