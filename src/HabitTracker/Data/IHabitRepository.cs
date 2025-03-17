namespace HabitTracker;

public interface IHabitRepository
{
    void InitializeDatabase();
    List<Habit> GetHabits();
    void AddHabit(Habit habit);
    void RemoveHabit(long habitId);
    void AddCompletion(long habitId, DateTime completionTime);
    void RemoveCompletion(long habitId, DateTime dateTime);
}
