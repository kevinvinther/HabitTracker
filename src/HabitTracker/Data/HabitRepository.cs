using HabitTracker;
using Microsoft.Data.Sqlite;

public class HabitRepository : IHabitRepository
{
    private readonly string _connectionString;

    public HabitRepository(string path = "habits.db")
    {
        _connectionString = $"Data Source={path}";
    }

    /// <summary>
    /// Creates the tables Habits and Completions, which contain respectively a habit, and the completions of a habit.
    /// </summary>
    public void InitializeDatabase()
    {
        using var connection = GetOpenConnection();

        using var createHabitsTableCmd = new SqliteCommand(@"
                CREATE TABLE IF NOT EXISTS Habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL
                );
                CREATE TABLE IF NOT EXISTS Completions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    HabitId INTEGER NOT NULL,
                    CompletionTime TEXT NOT NULL,
                    FOREIGN KEY (HabitId) REFERENCES Habits (Id) ON DELETE CASCADE
                );", connection);
        createHabitsTableCmd.ExecuteNonQuery();
    }

    private SqliteConnection GetOpenConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    private bool HabitExists(SqliteConnection connection, long habitId)
    {
        using var checkHabitCmd = new SqliteCommand("SELECT COUNT(*) FROM Habits WHERE Id = @HabitId", connection);
        checkHabitCmd.Parameters.AddWithValue("@HabitId", habitId);

        var count = Convert.ToInt32(checkHabitCmd.ExecuteScalar());

        return count > 0;
    }

    /// <summary>
    /// Gets a list of all the habits in the database.
    /// </summary>
    /// <returns>A List of all the habits in the database.</returns>
    public List<Habit> GetHabits()
    {
        var habits = new List<Habit>();

        using var connection = GetOpenConnection();

        using var getHabitsCmd = new SqliteCommand("SELECT * FROM Habits", connection);
        using var reader = getHabitsCmd.ExecuteReader();

        while (reader.Read())
        {
            var habit = new Habit(id: reader.GetInt32(0), name: reader.GetString(1));
            habit.SetCompletions(GetCompletions(habit.Id));
            habits.Add(habit);
        }
        return habits;
    }

    /// <summary>
    /// Gets all the completions from a specific habit from the database.
    /// </summary>
    /// <param name="habitId">The ID of the habit whose completions are returned.</param>
    /// <returns>The completions of a given habit.</returns>
    private List<DateTime> GetCompletions(long habitId)
    {
        using var connection = GetOpenConnection();

        using var getCompletionsCmd = new SqliteCommand(
            "SELECT * FROM Completions WHERE HabitId = @habitId;",
            connection);
        getCompletionsCmd.Parameters.AddWithValue("@habitId", habitId);

        var completions = new List<DateTime>();
        using var reader = getCompletionsCmd.ExecuteReader();
        while (reader.Read())
        {
            completions.Add(DateTime.Parse(reader.GetString(2)));
        }

        return completions;
    }

    /// <summary>
    /// Adds a new habit to the database.
    /// </summary>
    /// <param name="habit">The Habit to add to the database.</param>
    /// <returns>The ID of the entry of the habit in the database.</returns>
    public long AddHabit(Habit habit)
    {
        if (GetHabits().Any(h => h.Name == habit.Name))
        {
            throw new InvalidOperationException($"A habit with name '{habit.Name}' already exists.");
        }

        using var connection = GetOpenConnection();

        using var insertHabitCmd = new SqliteCommand(
            "INSERT INTO Habits (Name) VALUES (@name);", connection);
        insertHabitCmd.Parameters.AddWithValue("@name", habit.Name);
        insertHabitCmd.ExecuteNonQuery();

        using var lastIdCmd = new SqliteCommand("SELECT last_insert_rowid();", connection);
        var result = lastIdCmd.ExecuteScalar();

        habit.setId(result != null ? Convert.ToInt64(result) : -1);

        return habit.Id;
    }


    /// <summary>
    /// Remove a habit
    /// </summary>
    /// <param name="habitId">The ID of the habit to be removed.</param>
    public void RemoveHabit(long habitId)
    {
        using var connection = GetOpenConnection();

        if (!HabitExists(connection, habitId))
        {
            throw new InvalidOperationException($"Habit with ID {habitId} does not exist.");
        }

        using var deleteHabitCmd = new SqliteCommand("DELETE FROM Habits WHERE Id = @HabitId", connection);
        deleteHabitCmd.Parameters.AddWithValue("@HabitId", habitId);
        deleteHabitCmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Remove a completion from the database.
    /// </summary>
    /// <param name="habitId">The ID of the habit you want to remove a completion from.</param>
    /// <param name="dateTime">The DateTime of the completion you want to remove</param>
    public void RemoveCompletion(long habitId, DateTime dateTime)
    {
        var formattedDateTime = DateTimeHelper.Format(dateTime);

        using var connection = GetOpenConnection();

        using var deleteCompletionCmd = new SqliteCommand("DELETE FROM Completions WHERE HabitId = @HabitId AND CompletionTime = @Date", connection);
        deleteCompletionCmd.Parameters.AddWithValue("@HabitId", habitId);
        deleteCompletionCmd.Parameters.AddWithValue("@Date", formattedDateTime);
        int rowsAffected = deleteCompletionCmd.ExecuteNonQuery();

        if (rowsAffected == 0)
        {
            throw new InvalidOperationException($"No completion found for Habit ID {habitId} at {formattedDateTime}");
        }
    }


    /// <summary>
    /// Add a completion to the database.
    /// </summary>
    /// <param name="habitId">The ID of the habit you want to add a completion to.</param>
    /// <param name="completionTime">The DateTime of the completion.</param>
    public void AddCompletion(long habitId, DateTime completionTime)
    {
        using var connection = GetOpenConnection();

        using var insertHabitCmd = new SqliteCommand(@"
                INSERT INTO Completions (HabitId, CompletionTime)
                VALUES (@habitId, @CompletionTime);", connection);

        insertHabitCmd.Parameters.AddWithValue("@habitId", habitId);
        insertHabitCmd.Parameters.AddWithValue("@CompletionTime",
                                               DateTimeHelper.Format(completionTime));
        insertHabitCmd.ExecuteNonQuery();
    }
}
