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

        public long AddHabit(Habit habit) => _repository.AddHabit(habit);

        public void RemoveHabit(long habitId) => _repository.RemoveHabit(habitId);

        public void AddCompletion(long habitId, DateTime completionTime) =>
            _repository.AddCompletion(habitId, completionTime);

        public void RemoveCompletion(long habitId, DateTime completionTime) =>
            _repository.RemoveCompletion(habitId, completionTime);

        /// <summary>
        /// Gets the habits which have not been completed on a specific date.
        /// </summary>
        /// <param name="dt">The date you want to filter off</param>
        /// <returns>The habits whose newest completion was not today.</returns>
        public List<Habit> GetHabitsNotCompletedOnDay(DateTime dt)
            => GetHabits().Where(habit => !habit.Completions.Any(c => c.Date == dt.Date))
                .ToList();
    }
}
