using FluentAssertions;
using FluentAssertions.Execution;
using Library.Api.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels;
using Xunit;
using AutoFixture;

namespace UnitTests.Web.ViewModels
{
    public class LaunchViewModelTests
    {
        private readonly Fixture _fixture = new();
        
        [Fact]
        public void Date_ShouldBeOnlyDate_WhenHoursAndMinutesAreZero()
        {
            var launchResponse = _fixture.Build<LaunchResponse>().With(l => l.Date, new DateTime(2022, 1, 1)).Create();

            LaunchViewModel result = new(launchResponse);

            result.Date.Should().Be("01/01/2022");
        }

        [Fact]
        public void Date_ShouldBeFullDate_WhenHoursAndMinutesAreNotZero()
        {
            var launchResponse = _fixture.Build<LaunchResponse>().With(l => l.Date, new DateTime(2022, 1, 1, 1, 1, 1)).Create();

            LaunchViewModel result = new(launchResponse);

            result.Date.Should().Be("01/01/2022 01:01");
        }

        [Fact]
        public void Date_ShouldBeEmpty_WhenLaunchResponseDateIsNull()
        {
            var launchResponse = _fixture.Build<LaunchResponse>().Without(l => l.Date).Create();

            LaunchViewModel result = new(launchResponse);

            result.Date.Should().BeEmpty();
        }
    }
}
