using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Library.Api;
using Library.Services;
using FluentAssertions;
using Library.Api.Responses;
using AutoFixture;

namespace UnitTests.Library.Services
{
    public class ArticleServiceTests
    {
        private readonly ArticleService _articleService;
        private readonly Mock<IArticleApi> _articleApi = new();
        private readonly Fixture _fixture = new();

        public ArticleServiceTests()
        {
            _articleService = new ArticleService(_articleApi.Object);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 10)]
        public async Task GetArticlesAsync_ShouldReturnDifferentArticlesDependingOnPageNumber(int pageNumber, int start)
        {
            List<ArticleResponse> expected = new()
            {
                _fixture.Create<ArticleResponse>()
            };
            
            _articleApi.Setup(a => a.GetArticlesAsync("search", start)).Returns(Task.FromResult(expected));

            var result = await _articleService.GetArticlesAsync("search", pageNumber);

            result.Should().Equal(expected);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, 5)]
        [InlineData(1, 10)]
        [InlineData(2, 11)]
        public async Task GetPagesCountAsync_ShouldCalculatePagesCorrectly(int expected, int articles)
        {
            _articleApi.Setup(a => a.GetArticlesCountAsync("search")).Returns(Task.FromResult(articles));

            var pages = await _articleService.GetPagesCountAsync("search");

            pages.Should().Be(expected);
        }
    }
}
