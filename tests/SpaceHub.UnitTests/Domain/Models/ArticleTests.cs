using SpaceHub.Domain.Models;

namespace SpaceHub.UnitTests.Domain.Models;

public class ArticleTests
{
    private readonly Fixture _fixture = new();

    [Theory]
    [InlineData("mars", "123 456", "mars test", true)] // Only summary contains search value
    [InlineData("mars", "mars test", "123 456", true)] // Only title contains search value
    [InlineData("mars", "mars test", "test mars", true)] // Both title & summary contain search value
    [InlineData("mars", "test1", "smar", false)] // Neither title nor summary contain search value
    [InlineData("mars", "MaRs", "test", true)] // Case-insensitive
    public void ArticleMatchesSearchCriteria_Should_ReturnExpectedResult(string searchValue, string title, string summary, bool expected)
    {
        var article = _fixture.Build<Article>()
            .With(x => x.Title, title)
            .With(x => x.Summary, summary)
            .Create();
        var result = article.MatchesSearch(searchValue);

        result.Should().Be(expected);
    }
}
