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
                } else {
                    Habits.Add(habit);
                }
            }
        }

        /// <summary>
        /// Removes habits from the manager.
        /// </summary>
        /// <param name="habits">The habits to be removed</param>
        public void RemoveHabits(params Habit[] habits) {
            foreach (var habit in habits) {
                if (!Habits.Any(h => h.Name.Equals(habit.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"Habit doesn't exist in directory!");
                } else {
                    Habits.Remove(habit);
                }
            }
        }

        /// <summary>
        /// Add a completion time to the habit.
        /// </summary>
        /// <param name="habit">The habit you want to add a completion to.</param>
        /// <param name="dateTimes">The completion time.</param>
        public void AddCompletionTime(Habit habit, params DateTime[] dateTimes) {
            foreach (var t in dateTimes) {
                // No if here, if it for some reason bugs, you should rather
                // have two identical completions, than one completion when
                // you've actually done two.
                habit.Completions.Add(t);
            }
        }

        /// <summary>
        /// Remove a completion from specified habit.
        /// </summary>
        /// <param name="habit">The habit you want to remove a completion from.</param>
        /// <param name="dateTime">The completion time.</param>
        /// <returns></returns>
        public void RemoveCompletionTime(Habit habit, DateTime dateTime) {
            // Removes the first occurence
            habit.RemoveCompletion(dateTime);
        }
    }
}
