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
        public required string Task_Name { get; set; }
        public required string Date { get; set; }
    }

    public HabiticaImporter(IImportService importService)
    {
        _importService = importService;
    }

    public void ImportData(string filePath)
    {

        _importService.AddHabitsWithCompletions(ImportHabits(filePath));
    }

    public IEnumerable<Habit> ImportHabits(string filePath)
    {
        var csv = CsvHelper.GetCsvReader(filePath);


        var records = csv.GetRecords<ParsedHabit>();

        foreach (var habit in records)
        {
            HabitDictionaryHelper.AddHabitCompletion(ref habits, habit.Task_Name, habit.Date);
        }

        return HabitDictionaryHelper.ConvertToHabits(habits);
    }
}
