using System.Text;

namespace HabitTracker
{
    /// <summary>
    /// Represents a habit that can be tracked over time.
    /// </summary>
    public class Habit
    {
        public long Id { get; set; }
        public string Name { get; private set; }
        public List<DateTime> Completions { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Habit"/> class.
        /// </summary>
        /// <param name="id">The id of the habit</param>
        /// <param name="name">The name of the habit</param>
        /// <returns>A new instance of the <see cref="Habit"/> class.</returns>
        public Habit(long id, string name)
            : this(id, name, []) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Habit"/> class.
        /// </summary>
        /// <param name="id">The id of the habit</param>
        /// <param name="name">The name of the habit</param>
        /// <param name="completions">The completion dates of the habit</param>
        /// <returns>A new instance of the <see cref="Habit"/> class.</returns>
        public Habit(long id, string name, params DateTime[] completions)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty", nameof(name));
            }
            Id = id;
            Name = name;
            Completions = new List<DateTime>(completions);
        }

        /// <summary>
        /// Allows for the conversion of the habit to a string.
        /// </summary>
        public override string ToString()
        {
            return Name;
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
        /// Removes a completion date from the habit.
        /// </summary>
        /// <param name="date">The date-time of the completion.</param>
        /// <returns>True if successfully removed, false if not.</returns>
        public bool RemoveCompletion(DateTime date)
        {
            return Completions.Remove(date);
        }

        /// <summary>
        /// Returns a list of all completion dates, if they exist. If they don't
        /// returns a string stating this.
        /// </summary>
        public string GetCompletionDates()
        {
            StringBuilder completionDates = new StringBuilder("", 50);
            if (Completions.Count == 0)
            {
                return $"There are not yet any completions for habit {Name}! :(";
            }
            else
            {
                completionDates.Append($"Habit: {Name}\n");
                foreach (var completion in Completions)
                {
                    completionDates.Append($"* {completion}\n");
                }
            }
            return completionDates.ToString();
        }
    }
}
