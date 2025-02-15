namespace HabitTracker
{
    using Microsoft.Data.Sqlite;

    /// <summary>
    /// Manages habits for tracking.
    /// </summary>
    public class HabitManager
    {
        private readonly string _connectionString;


        public HabitManager(String location = "habits.db")
        {
            location = "Data Source=" + location;
            _connectionString = location;
        }

        /// <summary>
        /// Creates the tables Habits and Completions, which contain respectively a habit, and the completions of a habit.
        /// </summary>
        public void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            
            var createHabitsTableCmd = connection.CreateCommand();
            createHabitsTableCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL
                );
                CREATE TABLE IF NOT EXISTS Completions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    HabitId INTEGER NOT NULL,
                    CompletionTime TEXT NOT NULL,
                    FOREIGN KEY (HabitId) REFERENCES Habits (Id) ON DELETE CASCADE
                );
            ";
            createHabitsTableCmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Adds a new habit to the database.
        /// </summary>
        /// <param name="habit">The Habit to add to the database.</param>
        public void AddHabit(Habit habit)
        {
            if (GetHabits().Any(h => h.Name == habit.Name))
            {
                throw new InvalidOperationException($"A habit with name '{habit.Name}' already exists.");
            }
            
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var insertHabitCmd = connection.CreateCommand();
            insertHabitCmd.CommandText = "INSERT INTO Habits (Name) VALUES (@name);";
            insertHabitCmd.Parameters.AddWithValue("@name", habit.Name);
            insertHabitCmd.ExecuteNonQuery();

            var lastIdCmd = connection.CreateCommand();
            lastIdCmd.CommandText = "SELECT last_insert_rowid();";
            var result = lastIdCmd.ExecuteScalar();

            habit.Id = result != null ? (long)Convert.ToInt64(result) : -1;
        }


        /// <summary>
        /// Add a completion to the database.
        /// </summary>
        /// <param name="habitId">The ID of the habit you want to add a completion to.</param>
        /// <param name="completionTime">The DateTime of the completion.</param>
        public void AddCompletion(long habitId, DateTime completionTime) {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var insertHabitCmd = connection.CreateCommand();
            insertHabitCmd.CommandText = @"
                INSERT INTO Completions (HabitId, CompletionTime)
                VALUES (@habitId, @CompletionTime);
                ";
            insertHabitCmd.Parameters.AddWithValue("@habitId", habitId);
            insertHabitCmd.Parameters.AddWithValue("@CompletionTime", completionTime);
            insertHabitCmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Gets a list of all the habits in the database.
        /// </summary>
        /// <returns>A List<Habit> of all the habits in the database.</returns>
        public List<Habit> GetHabits() {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var getHabitsCmd = connection.CreateCommand();
            getHabitsCmd.CommandText = "SELECT * FROM Habits;";

            var habits = new List<Habit>();

            using var reader = getHabitsCmd.ExecuteReader();
            while (reader.Read()) {
                var habit = new Habit(id: reader.GetInt32(0), name: reader.GetString(1));
                habit.Completions = GetCompletions(habit.Id);
                habits.Add(habit);
            }

            return habits;
        }

        /// <summary>
        /// Gets all the completions from a specific habit from the database.
        /// </summary>
        /// <param name="habitId">The ID of the habit whose completions are returned.</param>
        /// <returns>The completions of a given habit.</returns>
        public List<DateTime> GetCompletions(long habitId) {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var getCompletionsCmd= connection.CreateCommand();
            getCompletionsCmd.CommandText = "SELECT * FROM Completions WHERE HabitId = @habitId;";
            getCompletionsCmd.Parameters.AddWithValue("@habitId", habitId);

            var completions = new List<DateTime>();
            using var reader = getCompletionsCmd.ExecuteReader();
            while (reader.Read()){
                completions.Add(DateTime.Parse(reader.GetString(2)));
            }

            return completions;
        }

        public void RemoveHabit(long habitId)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using (var deleteHabitCmd = connection.CreateCommand())
            {
                deleteHabitCmd.CommandText = "DELETE FROM Habits WHERE Id = @HabitId";
                deleteHabitCmd.Parameters.AddWithValue("@HabitId", habitId);
                deleteHabitCmd.ExecuteNonQuery();
            }
            
            using (var deleteCompletionsCmd = connection.CreateCommand())
            {
                deleteCompletionsCmd.CommandText = "DELETE FROM Completions WHERE HabitId = @HabitId";
                deleteCompletionsCmd.Parameters.AddWithValue("@HabitId", habitId);
                deleteCompletionsCmd.ExecuteNonQuery();
            }
        }

        public void RemoveCompletion(long habitId, DateTime dateTime)
        {
            string formattedDateTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff");    
            
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var insertHabitCmd = connection.CreateCommand();
            insertHabitCmd.CommandText = "DELETE FROM Completions WHERE CompletionTime == @Date";
            insertHabitCmd.Parameters.AddWithValue("@Date", formattedDateTime);
            insertHabitCmd.ExecuteNonQuery();
        }
    }
}
