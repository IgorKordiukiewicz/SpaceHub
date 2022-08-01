using FluentAssertions;
using Library.Services;
using Xunit;

namespace UnitTests.Library.Services;

public class HelpersTests
{
    [Fact]
    public void GetCacheKeyForRequestWithPages_ReturnsFormattedString()
    {
        var result = Helpers.GetCacheKeyForRequestWithPages("name", "search", 0, 10);

        result.Should().Be("name_search_0_10");
    }
}
