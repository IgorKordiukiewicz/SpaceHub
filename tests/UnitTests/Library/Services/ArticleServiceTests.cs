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
using FluentValidation;
using Library.Models;
using Library.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;

namespace UnitTests.Library.Services
{
    public class ArticleServiceTests
    {
        private readonly ArticleService _articleService;
        private readonly Mock<IArticleApi> _articleApi = new();
        private readonly Fixture _fixture = new();

        public ArticleServiceTests()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();

            _articleService = new ArticleService(_articleApi.Object, memoryCache);
        }

        [Fact]
        public async Task GetArticlesAsync_ShouldReturnArticlesList_WhenRequestIsValid()
        {
            List<ArticleResponse> expectedResponse = new()
            {
                _fixture.Create<ArticleResponse>()
            };

            List<Article> expected = expectedResponse.Select(a => a.ToModel()).ToList();

            string searchValue = "search";
            _articleApi.Setup(a => a.GetArticlesAsync(searchValue, 10, 0)).Returns(Task.FromResult<IEnumerable<ArticleResponse>>(expectedResponse));

            var result = await _articleService.GetArticlesAsync(searchValue, 1);

            result.Should().Equal(expected);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 10)]
        public async Task GetArticlesAsync_ShouldReturnDifferentArticles_DependingOnPageNumber(int pageNumber, int start)
        {
            List<ArticleResponse> expectedResponse = new()
            {
                _fixture.Create<ArticleResponse>()
            };

            List<Article> expected = expectedResponse.Select(a => a.ToModel()).ToList();

            string searchValue = "search";
            _articleApi.Setup(a => a.GetArticlesAsync(searchValue, 10, start)).Returns(Task.FromResult<IEnumerable<ArticleResponse>>(expectedResponse));

            var result = await _articleService.GetArticlesAsync(searchValue, pageNumber);

            result.Should().Equal(expected);
        }
    }
}
