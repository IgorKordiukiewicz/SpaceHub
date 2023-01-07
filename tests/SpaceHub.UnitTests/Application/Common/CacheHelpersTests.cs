using SpaceHub.Application.Common;

namespace SpaceHub.UnitTests.Application.Common;

public class CacheHelpersTests
{
    [Fact]
    public void GetCacheKeyForRequestWithPages_Should_GenerateCacheKeyCorrectly()
    {
        var result = CacheHelpers.GetCacheKeyForRequestWithPages("myCache", "abc", 0, 10);

        result.Should().Be("myCache_abc_0_10");
    }

    [Fact]
    public void GetCacheKeyForRequestWithPages_Should_GenerateCacheKeyCorrectly_WhenSearchValueIsNull()
    {
        var result = CacheHelpers.GetCacheKeyForRequestWithPages("myCache", null, 0, 10);

        result.Should().Be("myCache__0_10");
    }
}

