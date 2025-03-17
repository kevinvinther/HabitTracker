namespace HabitTracker;

public interface IImportService
{
    void AddHabitsWithCompletions(IEnumerable<Habit> habits);
    void AddCompletions(long habitId, IEnumerable<DateTime> completions);
}
