using Xunit;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using Library.Attributes;
using Library.Extensions;

namespace UnitTests.Library.Extensions;

public class EnumExtensionsTests
{
    public enum EnumType
    {
        [Display(Name = "Name")]
        [Symbol("s")]
        EnumValue1,

        EnumValue2,
    }

    [Fact]
    public void GetDisplayName_ShouldReturnDisplayName_WhenEnumHasAttribute()
    {
        var result = EnumType.EnumValue1.GetDisplayName();
        result.Should().Be("Name");
    }

    [Fact]
    public void GetDisplayName_ShouldReturnNull_WhenEnumDoesntHaveAttribute()
    {
        var result = EnumType.EnumValue2.GetDisplayName();
        result.Should().BeNull();
    }

    [Fact]
    public void GetSymbol_ShouldReturnSymbol_WhenEnumHasAttribute()
    {
        var result = EnumType.EnumValue1.GetSymbol();
        result.Should().Be("s");
    }

    [Fact]
    public void GetSymbol_ShouldReturnNull_WhenEnumDoesntHaveAttribute()
    {
        var result = EnumType.EnumValue2.GetSymbol();
        result.Should().BeNull();
    }
}
