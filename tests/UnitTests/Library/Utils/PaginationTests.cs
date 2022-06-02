using Library.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace UnitTests.Library.Utils
{
    public class PaginationTests
    {
        private readonly Pagination _pagination = new();

        [Theory]
        [InlineData(10, 1, 0)]
        [InlineData(10, 2, 10)]
        public void GetOffset_ShouldCalculateOffsetProperly(int itemsPerPage, int pageNumber, int expected)
        {
            _pagination.ItemsPerPage = itemsPerPage;

            var result = _pagination.GetOffset(pageNumber);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(10, 0, 1)]
        [InlineData(10, 5, 1)]
        [InlineData(10, 10, 1)]
        [InlineData(10, 11, 2)]
        public void GetPagesCount_ShouldCalculatePagesCountProperly(int itemsPerPage, int itemsCount, int expected)
        {
            _pagination.ItemsPerPage = itemsPerPage;

            var result = _pagination.GetPagesCount(itemsCount);

            result.Should().Be(expected);
        }
    }
}
