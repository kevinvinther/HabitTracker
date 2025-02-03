namespace HabitTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Habit habit = new Habit("Anki");
            habit.AddCompletion(DateTime.Now);
            habit.AddCompletion(DateTime.Today);
            habit.AddCompletion(DateTime.Today);
            habit.AddCompletion(DateTime.Today);
            habit.PrintCompletionDates();
        }
    }

    /// <summary>
    /// Represents a habit that can be tracked over time.
    /// </summary>
    public class Habit
    {
        public string Name { get; private set; }
        public List<DateTime> Completions { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Habit"/> class.
        /// </summary>
        /// <param name="name">The name of the habit</param>
        /// <returns>A new instance of the <see cref="Habit"/> class.</returns>
        public Habit(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty", nameof(name));
            }
            Name = name;
            Completions = new List<DateTime>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Habit"/> class.
        /// </summary>
        /// <param name="name">The name of the habit</param>
        /// <param name="completions">The completion dates of the habit</param>
        /// <returns>A new instance of the <see cref="Habit"/> class.</returns>
        public Habit(string name, params DateTime[] completions)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty", nameof(name));
            }
            Name = name;
            Completions = new List<DateTime>(completions);
        }

        /// <summary>
        /// Adds a completion date to the habit
        /// </summary>
        /// <param name="date">Completed date</param>
        public void AddCompletion(DateTime date)
        {
            Completions.Add(date);
        }

        /// <summary>
        /// Prints a list of all completion dates, if they exist.
        /// </summary>
        public void PrintCompletionDates()
        {
            if (Completions.Count == 0)
            {
                Console.WriteLine($"There are not yet any completions for habit {Name}! :(");
            }
            else
            {
                Console.WriteLine($"Habit: {Name}");
                foreach (var completion in Completions)
                {
                    Console.WriteLine($"* {completion}");
                }
            }
        }
    }
}

