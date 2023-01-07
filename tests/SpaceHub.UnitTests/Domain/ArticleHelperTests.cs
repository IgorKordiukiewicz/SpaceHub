using SpaceHub.Domain;

namespace SpaceHub.UnitTests.Domain;

public class ArticleHelperTests
{
    [Theory]
    [InlineData("mars", "123 456", "mars test", true)] // Only summary contains search value
    [InlineData("mars", "mars test", "123 456", true)] // Only title contains search value
    [InlineData("mars", "mars test", "test mars", true)] // Both title & summary contain search value
    [InlineData("mars", "test1", "smar", false)] // Neither title nor summary contain search value
    [InlineData("mars", "MaRs", "test", true)] // Case-insensitive
    public void ArticleMatchesSearchCriteria_Should_ReturnExpectedResult(string searchValue, string title, string summary, bool expected)
    {
        var result = ArticleHelper.ArticleMatchesSearchCriteria(searchValue, title, summary);

        result.Should().Be(expected);
    }
}
