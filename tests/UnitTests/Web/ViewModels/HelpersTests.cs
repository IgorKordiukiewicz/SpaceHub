using System;
using Xunit;
using Web.ViewModels;
using FluentAssertions;

namespace UnitTests.Web.ViewModels;

public class HelpersTests
{
    [Fact]
    public void DateToString_ShouldReturnOnlyDate_WhenHoursAndMinutesAreZero()
    {
        var result = Helpers.DateToString(new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Local));

        result.Should().Be("01/01/2022");
    }

    [Fact]
    public void DateToString_ShouldReturnFullDate_WhenHoursAndMinutesAreNotZero()
    {
        var result = Helpers.DateToString(new DateTime(2022, 1, 1, 1, 1, 1, DateTimeKind.Local));

        result.Should().Be("01/01/2022 01:01");
    }

    [Fact]
    public void DateToString_ShouldReturnOnlyDate_WhenOnlyDateParamIsTrue()
    {
        var result = Helpers.DateToString(new DateTime(2022, 1, 1, 1, 1, 1, DateTimeKind.Local), true);

        result.Should().Be("01/01/2022");
    }

    [Fact]
    public void DateToString_ShouldReturnNull_WhenDateTimeIsNull()
    {
        var result = Helpers.DateToString(null);

        result.Should().BeNull();
    }

    [Theory]
    [InlineData(2020, 1, 1, 0, 0, 0, 1577836800000)]
    [InlineData(1970, 1, 1, 0, 0, 5, 5000)]
    public void DateToJsMilliseconds_ShouldCalculateMillisecondsProperly(int year, int month, int day, int hour, int minute, int second, long expected)
    {
        var dateTime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);

        var result = Helpers.DateToJsMilliseconds(dateTime);

        result.Should().Be(expected);
    }

    [Fact]
    public void ValueToStringWithSymbol_ShouldReturnValueWithSymbolWithSpaceBetween_WhenValueIsNotNullAndSpaceBetweenIsTrue()
    {
        var result = Helpers.ValueToStringWithSymbol(5, "kg", true, "-");

        result.Should().Be("5 kg");
    }

    [Fact]
    public void ValueToStringWithSymbol_ShouldReturnValueWithSymbolWithoutSpaceBetween_WhenValueIsNotNullAndSpaceBetweenIsFalse()
    {
        var result = Helpers.ValueToStringWithSymbol(5, "kg", false, "-");

        result.Should().Be("5kg");
    }

    [Fact]
    public void ValueToStringWithSymbol_ShouldReturnPlaceholder_WhenValueIsNull()
    {
        int? value = null;

        var result = Helpers.ValueToStringWithSymbol(value, "kg", true, "-");

        result.Should().Be("-");
    }
}
