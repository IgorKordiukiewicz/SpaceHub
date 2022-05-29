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

namespace UnitTests.Web.ViewModels
{
    public class ArticleViewModelTests
    {
        [Fact]
        public void PropertiesAssignedProperly()
        {
            ArticleResponse articleResponse = new()
            {
                Title = "Title",
                Summary = "Summary",
                Url = "https://testUrl",
                ImageUrl = "https://testImgUrl",
                NewsSite = "NewsSite",
                PublishDate = new DateTime(2022, 1, 1, 1, 1, 1)
            };

            ArticleViewModel result = new(articleResponse);

            using (new AssertionScope())
            {
                result.Title.Should().Be("Title");
                result.Summary.Should().Be("Summary");
                result.Url.Should().Be("https://testUrl");
                result.ImageUrl.Should().Be("https://testImgUrl");
                result.NewsSite.Should().Be("NewsSite");
                result.PublishDate.Should().Be("01.01.2022");
            }
        }
    }
}
