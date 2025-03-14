namespace HabitTracker;

public class Import : IImportService
{
    HabitManager _manager;
    HabitRepository _repository;

    public Import(HabitRepository repository)
    {
        _repository = repository;
        _manager = new HabitManager(repository);
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
