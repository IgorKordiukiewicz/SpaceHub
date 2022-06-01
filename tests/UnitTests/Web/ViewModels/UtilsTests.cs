using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Web.ViewModels;
using FluentAssertions;

namespace UnitTests.Web.ViewModels
{
    public class UtilsTests
    {
        [Fact]
        public void DateToString_ShouldReturnOnlyDate_WhenHoursAndMinutesAreZero()
        {
            var result = Utils.DateToString(new DateTime(2022, 1, 1));

            result.Should().Be("01/01/2022");
        }

        [Fact]
        public void DateToString_ShouldReturnFullDate_WhenHoursAndMinutesAreNotZero()
        {
            var result = Utils.DateToString(new DateTime(2022, 1, 1, 1, 1, 1));

            result.Should().Be("01/01/2022 01:01");
        }

        [Fact]
        public void DateToString_ShouldReturnOnlyDate_WhenOnlyDateParamIsTrue()
        {
            var result = Utils.DateToString(new DateTime(2022, 1, 1), true);

            result.Should().Be("01/01/2022");
        }

        [Fact]
        public void DateToString_ShouldReturnNull_WhenDateTimeIsNull()
        {
            var result = Utils.DateToString(null);

            result.Should().BeNull();
        }

        [Fact]
        public void ValueToStringWithSymbol_ShouldReturnValueWithSymbolWithSpaceBetween_WhenValueIsNotNullAndSpaceBetweenIsTrue()
        {
            var result = Utils.ValueToStringWithSymbol(5, "kg", true, "-");

            result.Should().Be("5 kg");
        }

        [Fact]
        public void ValueToStringWithSymbol_ShouldReturnValueWithSymbolWithoutSpaceBetween_WhenValueIsNotNullAndSpaceBetweenIsFalse()
        {
            var result = Utils.ValueToStringWithSymbol(5, "kg", false, "-");

            result.Should().Be("5kg");
        }

        [Fact]
        public void ValueToStringWithSymbol_ShouldReturnPlaceholder_WhenValueIsNull()
        {
            int? value = null;

            var result = Utils.ValueToStringWithSymbol(value, "kg", true, "-");

            result.Should().Be("-");
        }
    }
}
