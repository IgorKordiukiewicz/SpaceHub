using FluentAssertions;
using Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Library.Services
{
    public class HelpersTests
    {
        [Fact]
        public void GetCacheKeyForRequestWithPages_ReturnsFormattedString()
        {
            var result = Helpers.GetCacheKeyForRequestWithPages("name", "search", 0, 10);

            result.Should().Be("name_search_0_10");
        }
    }
}
