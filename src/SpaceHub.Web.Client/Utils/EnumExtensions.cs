using MudBlazor;
using SpaceHub.Contracts.Attributes;
using SpaceHub.Contracts.Enums;
using System.Globalization;
using System.Reflection;

namespace SpaceHub.Web.Client.Utils;

public static class EnumExtensions
{
    public static string ToUserFriendlyString(this ERocketComparisonProperty property) => property switch
    {
        ERocketComparisonProperty.LiftoffMass => "Liftoff Mass",
        ERocketComparisonProperty.LiftoffThrust => "Liftoff Thrust",
        ERocketComparisonProperty.CostPerKgToLeo => "Cost per kg to LEO",
        ERocketComparisonProperty.CostPerKgToGeo => "Cost per kg to GEO",
        _ => property.ToString()
    };

    public static string ToRGBAString(this Color color, float alpha = 1.0F)
    {
        var alphaStr = alpha.ToString("0.0#", CultureInfo.InvariantCulture);
        return color switch
        {
            Color.Primary => $"rgba(119, 107, 231, {alphaStr})",
            Color.Secondary => $"rgba(255, 64, 129, {alphaStr})",
            Color.Tertiary => $"rgba(30, 200, 165, {alphaStr})",
            _ => "rgba(255, 255, 255, 1.0)"
        };
    }

    public static string GetUnit(this Enum enumValue)
    {
        return enumValue.GetType().GetMember(enumValue.ToString()).First().GetCustomAttribute<UnitAttribute>()?.Unit ?? string.Empty;
    }
}
