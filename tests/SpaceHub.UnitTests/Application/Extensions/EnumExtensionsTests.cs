using SpaceHub.Application.Attributes;
using SpaceHub.Application.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SpaceHub.UnitTests.Application.Extensions;

public class EnumExtensionsTests
{
    public enum TestEnum
    {
        [Display(Name = "Option 1")]
        Option1,
        Option2,
    }

    [Fact]
    public void GetDisplayName_Should_ReturnCorrectDisplayName()
    {
        var result = TestEnum.Option1.GetDisplayName();

        result.Should().Be("Option 1");
    }

    [Fact]
    public void GetDisplayName_Should_ReturnEnumValueName_WhenNoDisplayAttribute()
    {
        var result = TestEnum.Option2.GetDisplayName();

        result.Should().Be("Option2");
    }

    public enum TestEnumWithSymbol
    {
        [Symbol("ABC")]
        Option1,
        Option2,
    }

    [Fact]
    public void GetSymbol_Should_ReturnCorrectSymbol()
    {
        var result = TestEnumWithSymbol.Option1.GetSymbol();

        result.Should().Be("ABC");
    }

    [Fact]
    public void GetSymbol_Should_ReturnNull_WhenNoSymbolAttribute()
    {
        var result = TestEnumWithSymbol.Option2.GetSymbol();

        result.Should().BeNull();
    }
}

