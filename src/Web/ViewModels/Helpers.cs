using System.Globalization;

namespace Web.ViewModels;

public static class Helpers
{
    private static readonly long _startDateTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

    public static string? DateToString(DateTime? dateTime, bool onlyDate = false)
    {
        if (dateTime != null)
        {
            var date = dateTime.Value.ToLocalTime();
            if (date.Hour == 0 && date.Minute == 0 || onlyDate)
            {
                return date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                return date.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            }
        }
        else
        {
            return null;
        }
    }

    public static long DateToJsMilliseconds(DateTime dateTime)
    {
        return (dateTime.ToUniversalTime().Ticks - _startDateTicks) / 10000;
    }

    public static string ValueToStringWithSymbol<T>(T? value, string symbol, bool spaceBetween = true, string placeholder = "-")
    {
        if(value != null)
        {
            return value.ToString() + (spaceBetween ? " " : "") + symbol;
        }
        else
        {
            return placeholder;
        }
    }
}
