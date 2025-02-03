namespace HabitTracker
{
    /// <summary>
    /// Manages habits for tracking.
    /// </summary>
    public class HabitManager
    {
        public List<Habit> Habits { get; }

        /// <summary>
        /// Initializes a new instance of HabitTracker with no habits.
        /// </summary>
        /// <returns>A HabitTracker instance with no habits initialized.</returns>
        public HabitManager()
        {
            Habits = new List<Habit>();
        }

        /// <summary>
        /// Initializes a new instance of HabitTracker with specified habits from arguments.
        /// </summary>
        /// <returns>A HabitTracker instance with habits from arguments initalized.</returns>
        public HabitManager(params Habit[] habits)
        {
            Habits = new List<Habit>(habits);
        }

        /// <summary>
        /// Adds a new habits to the manager.
        /// </summary>
        /// <param name="habits">The habits to add.</param>
        public void AddHabits(params Habit[] habits)
        {
            foreach (var habit in habits)
            {
                if (Habits.Any(h => h.Name.Equals(habit.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"A Habit with name '{habit.Name}' already exists.");
                }
                Habits.Add(habit);
            }
        }
    }
}
