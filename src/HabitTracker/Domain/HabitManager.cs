namespace HabitTracker
{
    /// <summary>
    /// Manages habits for tracking.
    /// </summary>
    public class HabitManager
    {
        private readonly IHabitRepository _repository;

        public HabitManager(IHabitRepository repository)
        {
            _repository = repository;
        }

        public void InitializeDatabase() => _repository.InitializeDatabase();

        public List<Habit> GetHabits() => _repository.GetHabits();

        public void AddHabit(Habit habit) => _repository.AddHabit(habit);

        public void RemoveHabit(long habitId) => _repository.RemoveHabit(habitId);

        public void AddCompletion(long habitId, DateTime completionTime) =>
            _repository.AddCompletion(habitId, completionTime);

        public void RemoveCompletion(long habitId, DateTime completionTime) =>
            _repository.RemoveCompletion(habitId, completionTime);
    }
}
