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

        private int[]? _streakCache;
        private bool _isCacheValid;

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
            : this(id, name, [])
        {
        }

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

        private void InvalidateCache()
        {
            _streakCache = null;
            _isCacheValid = false;
        }

        /// <summary>
        /// Adds a completion date to the habit
        /// </summary>
        /// <param name="date">Completed date</param>
        public void AddCompletion(DateTime date)
        {
            _completions.Add(date);
            InvalidateCache();
        }

        /// <summary>
        /// Removes a completion date from the habit.
        /// </summary>
        /// <param name="date">The date-time of the completion.</param>
        /// <returns>True if successfully removed, false if not.</returns>
        public bool RemoveCompletion(DateTime date)
        {
            var removed = _completions.Remove(date);

            if (removed)
                InvalidateCache();

            return removed;
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
        /// Returns a list of all completion dates, if they exist. If they don't,
        /// returns a string stating this.
        /// </summary>
        public string GetCompletionDates()
        {
            if (!_completions.Any())
                return $"There are not yet any completions for habit {_name}! :(";

            StringBuilder completionDates = new StringBuilder("", 50);

            completionDates.Append($"Habit: {_name}\n");
            foreach (var completion in _completions)
            {
                completionDates.AppendLine($"* {DateTimeHelper.Format(completion)}");
            }

            return completionDates.ToString();
        }

        /// <summary>
        /// Returns an array counting the streak, where the index $i$ is the
        /// streak at day $i$ after the first completion. Thus, if the habit had
        /// three completions, followed by a weeks break,
        /// ComputeStreakArray()[0..2] = [1,2,3], and
        /// ComputeStreakArray()[4..11] = [0,0...].
        /// If the user then resumes their streak, one day after their week break,
        /// it will continue counting from 1. E.g. GetStreakDay[12] = 1, etc.
        /// </summary>
        /// <returns>An array containing the streak at $i$ days after first
        /// completion day</returns>
        private int[] ComputeStreakArray()
        {
            var streaks = new List<int>();
            var currentStreak = 0;

            if (!_completions.Any())
                return [0];

            var startDate = _completions.Min().Date;
            var completionDates = _completions.Select(d => d.Date).ToList();

            for (var date = startDate; date <= DateTime.Today; date = date.AddDays(1))
            {
                if (completionDates.Contains(date))
                {
                    currentStreak += 1;
                }
                else
                {
                    currentStreak = 0;
                }

                streaks.Add(currentStreak);
            }

            return streaks.ToArray();
        }

        private int[] GetStreakArray()
        {
            if (!_isCacheValid || _streakCache == null)
            {
                _streakCache = ComputeStreakArray();
                _isCacheValid = true;
            }

            return _streakCache;
        }

        public int GetCurrentStreak()
        {
            return GetStreakArray().Last();
        }

        public int GetLongestStreak()
        {
            return GetStreakArray().Max();
        }

        public void SetId(long id)
        {
            _id = id;
        }
    }
}