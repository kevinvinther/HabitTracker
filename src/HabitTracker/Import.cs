namespace HabitTracker;

public class Import : IImportService
{
    HabitManager _manager;

    public Import(HabitManager manager)
    {
        _manager = manager;
    }

    public void AddHabitsWithCompletions(IEnumerable<Habit> habits)
    {
        foreach (var habit in habits)
        {
            _manager.AddHabit(habit);
            AddCompletions(habit.Id, habit.Completions);
        }
    }

    public void AddCompletions(long habitId, IEnumerable<DateTime> completions)
    {
        foreach (var completion in completions)
        {
            _manager.AddCompletion(habitId, completion);
        }
    }

}
