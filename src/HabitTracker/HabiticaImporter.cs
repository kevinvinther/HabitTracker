using System.Globalization;
using CsvHelper;

namespace HabitTracker;


/// <summary>
/// A class that given the user data CSV, imports the user data to the format
/// used in this habit tracker.
/// </summary>
public class HabiticaImporter : IImporter
{
    private readonly IImportService _importService;
    private Dictionary<string, HashSet<string>> habits = new Dictionary<string, HashSet<string>>();

    private class ParsedHabit
    {
        public string Task_Name { get; set; }
        public string Date { get; set; }
    }

    public HabiticaImporter(IImportService importService)
    {
        _importService = importService;
    }

    public void ImportData(string filePath)
    {

        _importService.AddHabitsWithCompletions(ImportHabits(filePath));
    }

    private void AddToDictionary(string habitName, string date)
    {
        if (!habits.TryGetValue(habitName, out var hashSet))
        {
            hashSet = new HashSet<string>();
            habits[habitName] = hashSet;
        }
        hashSet.Add(date);
    }

    private IEnumerable<Habit> DictionaryToHabits(Dictionary<string, HashSet<string>> dictionary)
    {
        List<Habit> habits = new List<Habit>();


        foreach (var habit in dictionary)
        {
            var dts = ParseDateStrings(habit.Value);

            habits.Add(new Habit(0, habit.Key, dts.ToArray()));
        }

        return habits;
    }

    private List<DateTime> ParseDateStrings(IEnumerable<string> date_list)
    {
        List<DateTime> dts = new List<DateTime>();

        foreach (var date in date_list)
        {
            dts.Add(DateTimeHelper.Parse(date));
        }

        return dts;
    }

    public IEnumerable<Habit> ImportHabits(string filePath)
    {
        var csv = GetCsvContent(filePath)
            ?? throw new ArgumentException("The file is empty, or another error has occured: CSV Content was null");


        var records = csv.GetRecords<ParsedHabit>();

        foreach (var habit in records)
        {
            AddToDictionary(habit.Task_Name, habit.Date);
        }

        return DictionaryToHabits(habits);
    }

    private CsvReader? GetCsvContent(string filePath)
    {
        var reader = new StreamReader(filePath);
        return new CsvReader(reader, CultureInfo.InvariantCulture);
    }
}
