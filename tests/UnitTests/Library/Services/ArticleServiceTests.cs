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
using Library.Api.Requests;
using FluentValidation;
using Library.Validators;

namespace UnitTests.Library.Services
{
    public class ArticleServiceTests
    {
        private readonly ArticleService _articleService;
        private readonly Mock<IArticleApi> _articleApi = new();
        private readonly Fixture _fixture = new();
        private readonly IValidator<ArticleRequest> _validator = new ArticleRequestValidator();

        public ArticleServiceTests()
        {
            _articleService = new ArticleService(_articleApi.Object, _validator);
        }

        [Fact]
        public async Task GetArticlesAsync_ShouldReturnEmptyList_WhenRequestIsInvalid()
        {
            var result = await _articleService.GetArticlesAsync(new ArticleRequest { PageNumber = -1 });

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetArticlesAsync_ShouldReturnArticlesList_WhenRequestIsValid()
        {
            List<ArticleResponse> expected = new()
            {
                _fixture.Create<ArticleResponse>()
            };

            string searchValue = "search";
            _articleApi.Setup(a => a.GetArticlesAsync(searchValue, 0)).Returns(Task.FromResult(expected));

            var result = await _articleService.GetArticlesAsync(new ArticleRequest { SearchValue = searchValue, PageNumber = 1 });

            result.Should().Equal(expected);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 10)]
        public async Task GetArticlesAsync_ShouldReturnDifferentArticles_DependingOnPageNumber(int pageNumber, int start)
        {
            List<ArticleResponse> expected = new()
            {
                _fixture.Create<ArticleResponse>()
            };

            string searchValue = "search";
            _articleApi.Setup(a => a.GetArticlesAsync(searchValue, start)).Returns(Task.FromResult(expected));

            var result = await _articleService.GetArticlesAsync(new ArticleRequest { SearchValue = searchValue, PageNumber = pageNumber});

            result.Should().Equal(expected);
        }
    }
}
