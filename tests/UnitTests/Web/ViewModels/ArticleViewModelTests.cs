using Library.Api.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels;
using Xunit;
using FluentAssertions;
using FluentAssertions.Execution;
using AutoFixture;

namespace UnitTests.Web.ViewModels
{
    public class ArticleViewModelTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public void Date_ShouldBeOnlyDate()
        {
            var articleResponse = _fixture.Build<ArticleResponse>().With(a => a.PublishDate, new DateTime(2022, 1, 1, 1, 1, 1)).Create();

            ArticleViewModel result = new(articleResponse);

            result.PublishDate.Should().Be("01/01/2022");
        }
    }
}
