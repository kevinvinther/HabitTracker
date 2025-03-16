using System.Globalization;

namespace HabitTracker;

public static class DateTimeHelper
{
    public const string DateFormat = "yyyy-MM-dd HH:mm:ss";

    public static string Format(DateTime dt) =>
        dt.ToString(DateFormat, CultureInfo.InvariantCulture);

    public static DateTime Parse(String dt) =>
        DateTime.ParseExact(dt, DateFormat, CultureInfo.InvariantCulture);

    public static List<DateTime> ParseDateStrings(IEnumerable<string> date_list)
    {
        List<DateTime> dts = new List<DateTime>();

        foreach (var date in date_list)
        {
            dts.Add(DateTimeHelper.Parse(date));
        }

        return dts;
    }
}
