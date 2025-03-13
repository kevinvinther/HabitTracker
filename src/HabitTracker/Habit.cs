using System.Globalization;
using System.Text;

namespace HabitTracker
{
    /// <summary>
    /// Represents a habit that can be tracked over time.
    /// </summary>
    public class Habit
    {
        private List<DateTime> _completions;
        private string _name;
        private long _id;

        public long Id => _id;
        public string Name => _name;
        public List<DateTime> Completions => _completions;


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
            _id = id;
            _name = name;
            _completions = new List<DateTime>(completions);
        }

        /// <summary>
        /// Allows for the conversion of the habit to a string.
        /// </summary>
        public override string ToString()
        {
            return _name;
        }

        /// <summary>
        /// Adds a completion date to the habit
        /// </summary>
        /// <param name="date">Completed date</param>
        public void AddCompletion(DateTime date)
        {
            _completions.Add(date);
        }

        /// <summary>
        /// Removes a completion date from the habit.
        /// </summary>
        /// <param name="date">The date-time of the completion.</param>
        /// <returns>True if successfully removed, false if not.</returns>
        public bool RemoveCompletion(DateTime date)
        {
            return _completions.Remove(date);
        }

        /// <summary>
        /// Setter method to set the completions to specified parameter.
        /// </summary>
        /// <param name="completions">The completion value to set completion to</param>
        public void SetCompletions(List<DateTime> completions)
        {
            _completions = completions;
        }

        /// <summary>
        /// Returns a list of all completion dates, if they exist. If they don't
        /// returns a string stating this.
        /// </summary>
        public string GetCompletionDates()
        {
            if (!_completions.Any())
                return $"There are not yet any completions for habit {_name}! :(";

            StringBuilder completionDates = new StringBuilder("", 50);

            string iso8601ish = "yyyy-MM-dd HH:mm:ss";
            completionDates.Append($"Habit: {_name}\n");
            foreach (var completion in _completions)
            {
                completionDates.AppendLine($"* {completion.ToString(iso8601ish, CultureInfo.InvariantCulture)}");
            }

            return completionDates.ToString();
        }

        public void setId(long id)
        {
            _id = id;
        }
    }
}
