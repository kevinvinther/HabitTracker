namespace HabitTracker;

public interface IImporter
{
    void ImportData(string filePath);
    IEnumerable<Habit> ImportHabits(string filePath);
}
