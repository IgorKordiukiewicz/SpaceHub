using SpaceHub.Infrastructure.Extensions;

namespace SpaceHub.UnitTests.Infrastructure.Extensions;

public class ApiHelpersTests
{
    [Theory]
    [InlineData(0, 10, 0)]
    [InlineData(5, 10, 1)]
    [InlineData(10, 10, 1)]
    [InlineData(11, 10, 2)]
    public void GetRequiredRequestsCount_Should_CalculateRequiredRequestsCountCorrectly(int itemsCount, int maxItemsPerRequest, int expectedResult)
    {
        var result = ApiHelpers.GetRequiredRequestsCount(itemsCount, maxItemsPerRequest);

        result.Should().Be(expectedResult);
    }
}
