using SpaceHub.Infrastructure.Api;

namespace SpaceHub.UnitTests.Infrastructure.Api;

public class ApiHelpersTests
{
    [Theory]
    [InlineData(2022, 1, 1, 10, 15, 0, "2022-01-01T10:15:00")]
    [InlineData(2020, 12, 31, 23, 59, 59, "2020-12-31T23:59:59")]
    public void ToQueryParameter_Should_FormatDateTimeAsExpected(int year, int month, int day, int hour, int minute, int second, string expected)
    {
        var dateTime = new DateTime(year, month, day, hour, minute, second);

        var result = dateTime.ToQueryParameter();

        result.Should().Be(expected);
    }

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
