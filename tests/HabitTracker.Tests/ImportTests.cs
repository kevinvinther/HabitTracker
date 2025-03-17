namespace HabitTracker.Tests;

[Collection("Sequential")]
public class ImportTests
{

    HabitRepository _repo;
    HabitManager _manager;
    IImportService _importer;

    public ImportTests()
    {
        _repo = new HabitRepository();
        _manager = new HabitManager(_repo);
        _importer = new Import(_manager);

        _repo.InitializeDatabase();
    }

    private Habit GetFirstHabit()
    {
        var habit = _manager.GetHabits().FirstOrDefault() ??
            throw new Exception("Added habit does not exist. This should have been covered in an earlier test. Critical failure.");
        return habit;
    }

    [Fact]
    public void AddCompletions_ShouldAdd_OneCompletion()
    {
        var habit = new Habit(0, "Exercise");

        _manager.AddHabit(habit);

        habit = GetFirstHabit();

        Assert.Empty(habit.Completions);

        var completions = new List<DateTime> {
            new DateTime(2024, 1, 1, 0,0,0),
        };

        _importer.AddCompletions(habit.Id, completions);

        habit = GetFirstHabit();

        Assert.Single(habit.Completions);

        Assert.Equal(new DateTime(2024, 1, 1, 0, 0, 0), habit.Completions.FirstOrDefault());

        _manager.RemoveHabit(habit.Id);
    }

    [Fact]
    public void AddCompletions_ShouldAdd_ManyCompletions()
    {
        var habit = new Habit(0, "Exercise");

        _manager.AddHabit(habit);

        habit = GetFirstHabit();

        Assert.Empty(habit.Completions);

        var completions = new List<DateTime> {
            new DateTime(2024, 1, 1, 0,0,0),
            new DateTime(2024, 1, 2, 1,0,0),
            new DateTime(2024, 1, 3, 2,0,0),
            new DateTime(2024, 1, 4, 3,0,0),
            new DateTime(2024, 1, 5, 4,0,0),
            new DateTime(2024, 1, 6, 5,0,0),
        };

        _importer.AddCompletions(habit.Id, completions);

        habit = GetFirstHabit();

        Assert.Equal(6, habit.Completions.Count);

        Assert.Equal(completions, habit.Completions);

        _manager.RemoveHabit(habit.Id);
    }

    [Fact]
    public void AddCompletions_ShouldNotThrow_WithNoCompletinos()
    {
        var habit = new Habit(0, "Exercise");

        _manager.AddHabit(habit);

        habit = GetFirstHabit();

        Assert.Empty(habit.Completions);

        var completions = new List<DateTime>();

        _importer.AddCompletions(habit.Id, completions);

        habit = GetFirstHabit();

        Assert.Empty(habit.Completions);

        _manager.RemoveHabit(habit.Id);
    }

    [Fact]
    public void AddHabitsWithCompletions_ShouldAdd_WithNoCompletions()
    {
        var habit = new Habit(0, "Exercise");

        Assert.Empty(_manager.GetHabits());

        _importer.AddHabitsWithCompletions(new List<Habit> { habit });

        Assert.Single(_manager.GetHabits());

        Assert.Empty(GetFirstHabit().Completions);

        _manager.RemoveHabit(habit.Id);
    }

    [Fact]
    public void AddHabitsWithCompletions_ShouldAdd_WithOneCompletion()
    {

        var completions = new List<DateTime> {
            new DateTime(2024, 1, 1, 0, 0, 0),
        };

        var habit = new Habit(0, "Exercise", completions.ToArray());

        Assert.Empty(_manager.GetHabits());

        _importer.AddHabitsWithCompletions(new List<Habit> { habit });

        Assert.Single(_manager.GetHabits());

        habit = GetFirstHabit();

        Assert.Single(habit.Completions);
        Assert.Equal(habit.Completions, completions);

        _manager.RemoveHabit(habit.Id);
    }

    [Fact]
    public void AddHabitsWithCompletions_ShouldAdd_WithManyCompletions()
    {

        var completions = new List<DateTime> {
            new DateTime(2024, 3, 1, 0, 0, 0),
            new DateTime(2024, 1, 2, 0, 0, 0),
            new DateTime(2024, 1, 3, 3, 23, 0),
            new DateTime(2025, 8, 1, 0, 0, 0),
            new DateTime(2026, 9, 1, 0, 9, 0),
        };

        var habit = new Habit(0, "Exercise", completions.ToArray());

        Assert.Empty(_manager.GetHabits());

        _importer.AddHabitsWithCompletions(new List<Habit> { habit });

        Assert.Single(_manager.GetHabits());

        habit = GetFirstHabit();

        Assert.Equal(5, habit.Completions.Count);
        Assert.Equal(habit.Completions, completions);

        _manager.RemoveHabit(habit.Id);
    }

    [Fact]
    public void AddHabitsWithCompletions_ShouldAdd_WithManyHabits_WithNoCompletions()
    {
        var habits = new List<Habit>
        {
            new Habit(0, "Exercise"),
            new Habit(0, "Reading"),
            new Habit(0, "Meditation")
        };

        Assert.Empty(_manager.GetHabits());

        _importer.AddHabitsWithCompletions(habits);

        Assert.Equal(3, _manager.GetHabits().Count);

        foreach (var habit in _manager.GetHabits())
        {
            Assert.Empty(habit.Completions);
            _manager.RemoveHabit(habit.Id);
        }
    }

    [Fact]
    public void AddHabitsWithCompletions_ShouldAdd_WithManyHabits_WithVaryingCompletions()
    {
        var habits = new List<Habit>
        {
            new Habit(0, "Exercise", new List<DateTime> { new DateTime(2024, 1, 1) }.ToArray()),
            new Habit(0, "Reading"),
            new Habit(0, "Meditation", new List<DateTime> { new DateTime(2024, 2, 1), new DateTime(2024, 2, 2) }.ToArray())
        };

        Assert.Empty(_manager.GetHabits());

        _importer.AddHabitsWithCompletions(habits);

        Assert.Equal(3, _manager.GetHabits().Count);

        var retrievedHabits = _manager.GetHabits().ToList();
        Assert.Single(retrievedHabits[0].Completions);
        Assert.Empty(retrievedHabits[1].Completions);
        Assert.Equal(2, retrievedHabits[2].Completions.Count);

        foreach (var habit in retrievedHabits)
        {
            _manager.RemoveHabit(habit.Id);
        }
    }

    [Fact]
    public void AddHabitsWithCompletions_ShouldAdd_WithManyHabits_WithManyCompletions()
    {
        var habits = new List<Habit>
        {
            new Habit(0, "Exercise", new List<DateTime> { new DateTime(2024, 1, 1), new DateTime(2024, 1, 2) }.ToArray()),
            new Habit(0, "Reading", new List<DateTime> { new DateTime(2024, 2, 1), new DateTime(2024, 2, 2), new DateTime(2024, 2, 3) }.ToArray()),
            new Habit(0, "Meditation", new List<DateTime> { new DateTime(2024, 3, 1), new DateTime(2024, 3, 2), new DateTime(2024, 3, 3), new DateTime(2024, 3, 4) }.ToArray())
        };

        Assert.Empty(_manager.GetHabits());

        _importer.AddHabitsWithCompletions(habits);

        Assert.Equal(3, _manager.GetHabits().Count);

        var retrievedHabits = _manager.GetHabits().ToList();
        Assert.Equal(2, retrievedHabits[0].Completions.Count);
        Assert.Equal(3, retrievedHabits[1].Completions.Count);
        Assert.Equal(4, retrievedHabits[2].Completions.Count);

        foreach (var habit in retrievedHabits)
        {
            _manager.RemoveHabit(habit.Id);
        }
    }
}
