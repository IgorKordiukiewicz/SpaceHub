using SpaceHub.Application.Common;

namespace SpaceHub.UnitTests.Application.Common;

public class PaginationTests
{
    [Theory]
    [InlineData(1, 10, 0)]
    [InlineData(2, 10, 10)]
    [InlineData(3, 10, 20)]
    public void GetOffset_Should_CalculateOffsetCorrectly(int pageNumber, int itemsPerPage, int expectedResult)
    {
        var result = Pagination.GetOffset(pageNumber, itemsPerPage);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(0, 10, 1)]
    [InlineData(9, 10, 1)]
    [InlineData(10, 10, 1)]
    [InlineData(11, 10, 2)]
    [InlineData(19, 10, 2)]
    public void GetPagesCount_Should_CalculatePagesCountCorrectly(int itemsCount, int itemsPerPage, int expectedResult)
    {
        var result = Pagination.GetPagesCount(itemsCount, itemsPerPage);

        result.Should().Be(expectedResult);
    }
}
