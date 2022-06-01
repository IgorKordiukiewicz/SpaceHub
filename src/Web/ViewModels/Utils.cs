using System.Globalization;

namespace Web.ViewModels
{
    public static class Utils
    {
        public static string? DateToString(DateTime? dateTime, bool onlyDate = false)
        {
            if (dateTime != null)
            {
                var date = dateTime.Value;
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
}
