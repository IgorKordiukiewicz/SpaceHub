using Library.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Library.Extensions;

public static class EnumExtensions
{
    public static string? GetDisplayName(this Enum enumValue)
    {
        return enumValue.GetType().GetMember(enumValue.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.GetName();
    }

    public static string? GetSymbol(this Enum enumValue)
    {
        return enumValue.GetType().GetMember(enumValue.ToString()).First().GetCustomAttribute<SymbolAttribute>()?.Symbol;
    }
}
