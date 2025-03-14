using System.Globalization;

public class DateTimeHelper
{
    public const string DateFormat = "yyyy-MM-dd HH:mm:ss";

    public static string Format(DateTime dt) =>
        dt.ToString(DateFormat, CultureInfo.InvariantCulture);
}
