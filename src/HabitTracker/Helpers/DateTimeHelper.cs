using System.Globalization;
using System.Text.RegularExpressions;

namespace HabitTracker;

public static class DateTimeHelper
{
    private static readonly Regex DateTimeRegex = new Regex(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$", RegexOptions.Compiled);
    private static readonly Regex DateOnlyRegex = new Regex(@"^\d{4}-\d{2}-\d{2}$", RegexOptions.Compiled);

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

    public static DateTime? TryParseUserDate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        try
        {
            if (DateOnlyRegex.IsMatch(input))
                return DateTimeHelper.Parse(input + " 00:00:00");
            if (DateTimeRegex.IsMatch(input))
                return DateTimeHelper.Parse(input);
        }
        catch (FormatException)
        {
            return null;
        }

        return null;

    }
}
