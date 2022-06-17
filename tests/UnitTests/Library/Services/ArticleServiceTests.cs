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
using FluentAssertions.Execution;
using Library.Data;

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

            var memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();

            _articleService = new ArticleService(_articleApi.Object, memoryCache);
        }

        [Fact]
        public async Task GetArticlesAsync_ShouldReturnDifferentArticles_DependingOnPageNumber()
        {
            List<ArticleResponse> expectedResponse1 = new() {_fixture.Create<ArticleResponse>() };
            List<ArticleResponse> expectedResponse2 = new() {_fixture.Create<ArticleResponse>() };

            List<Article> expected1 = expectedResponse1.Select(a => a.ToModel()).ToList();
            List<Article> expected2 = expectedResponse2.Select(a => a.ToModel()).ToList();

            string searchValue = "search";
            int itemsPerPage = _articleService.Pagination.ItemsPerPage;
            _articleApi.Setup(a => a.GetArticlesAsync(searchValue, itemsPerPage, 0)).Returns(Task.FromResult<IEnumerable<ArticleResponse>>(expectedResponse1));
            _articleApi.Setup(a => a.GetArticlesAsync(searchValue, itemsPerPage, itemsPerPage)).Returns(Task.FromResult<IEnumerable<ArticleResponse>>(expectedResponse2));

            var result1 = await _articleService.GetArticlesAsync(searchValue, 1);
            var result2 = await _articleService.GetArticlesAsync(searchValue, 2);

            using (new AssertionScope())
            {
                result1.Should().Equal(expected1);
                result2.Should().Equal(expected2);

                result1.Should().NotEqual(result2);
            }
        }

        [Fact]
        public async Task GetArticleAsync_ShouldReturnArticle()
        {
            ArticleResponse expectedResponse = _fixture.Create<ArticleResponse>();
            var expected = expectedResponse.ToModel();

            _articleApi.Setup(a => a.GetArticleAsync(1)).Returns(Task.FromResult(expectedResponse));

            var result = await _articleService.GetArticleAsync(1);
            result.Should().Be(expected);
        }
    }
}
