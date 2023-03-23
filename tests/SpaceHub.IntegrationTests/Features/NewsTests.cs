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

        _fixture.InitDb();
    }

    [Fact]
    public async Task GetNews_ShouldReturnArticlesOrderedByDate_WhenSearchValueIsEmpty()
    {
        var articles = await _fixture.GetAsync<ArticleModel>();
        var pagination = new Pagination();
        var result = await _fixture.SendRequest(new GetNewsQuery(string.Empty, pagination));

        using(new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Value.TotalPagesCount.Should().Be(pagination.GetPagesCount(articles.Count));
            result.Value.Articles.Count.Should().Be(pagination.ItemsPerPage);

            var expectedArticles = articles.OrderByDescending(x => x.PublishDate).Select(x => x.Title).Take(pagination.ItemsPerPage).ToList();
            var actualArticles = result.Value.Articles.OrderByDescending(x => x.PublishDate).Select(x => x.Title).ToList();
            actualArticles.Should().BeEquivalentTo(expectedArticles, options => options.WithStrictOrdering());
        }
    }

    [Fact]
    public async Task GetNews_ShouldReturnArticlesMatchingSearchCriteria_WhenSearchValueIsProvided()
    {
        var articles = await _fixture.GetAsync<ArticleModel>();
        var expectedMatchingArticle = articles[0];
        var pagination = new Pagination();
        var result = await _fixture.SendRequest(new GetNewsQuery(expectedMatchingArticle.Title, pagination));

        using(new AssertionScope())
        {
            result.IsSuccess.Should().BeTrue();
            result.Value.TotalPagesCount.Should().Be(1);
            result.Value.Articles.Count.Should().Be(1);
            result.Value.Articles[0].Title.Should().Be(expectedMatchingArticle.Title);
        }
    }
}
