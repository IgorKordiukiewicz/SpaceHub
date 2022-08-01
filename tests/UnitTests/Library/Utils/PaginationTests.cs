using Library.Utils;
using Xunit;
using FluentAssertions;

namespace UnitTests.Library.Utils;

public class PaginationTests
{
    [Theory]
    [InlineData(10, 1, 0)]
    [InlineData(10, 2, 10)]
    public void GetOffset_ShouldCalculateOffsetProperly(int itemsPerPage, int pageNumber, int expected)
    {
        var result = Pagination.GetOffset(pageNumber, itemsPerPage);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(10, 0, 1)]
    [InlineData(10, 5, 1)]
    [InlineData(10, 10, 1)]
    [InlineData(10, 11, 2)]
    public void GetPagesCount_ShouldCalculatePagesCountProperly(int itemsPerPage, int itemsCount, int expected)
    {
        var result = Pagination.GetPagesCount(itemsCount, itemsPerPage);

        result.Should().Be(expected);
    }
}
