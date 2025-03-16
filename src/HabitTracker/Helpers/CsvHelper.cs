using System.Globalization;
using CsvHelper;

namespace HabitTracker;

public static class CsvHelper
{
    public static CsvReader? GetCsvReader(string filePath)
    {
        var reader = new StreamReader(filePath);
        return new CsvReader(reader, CultureInfo.InvariantCulture);
    }
}
