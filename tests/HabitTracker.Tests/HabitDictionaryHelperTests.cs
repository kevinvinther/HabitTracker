namespace HabitTracker.Tests;

public class HabitDictionaryHelperTests
{
    [Fact]
    public void AddHabitCompletion_ShouldAddToExistingHabit()
    {
        var habits = new Dictionary<string, HashSet<string>> {
            { "Reading", new HashSet<string> { "2024-02-01 00:00:00" } }
        };

        HabitDictionaryHelper.AddHabitCompletion(ref habits, "Reading", "2024-02-02 00:00:00");

        Assert.Contains("2024-02-01 00:00:00", habits["Reading"]);
        Assert.Contains("2024-02-02 00:00:00", habits["Reading"]);
    }

    [Fact]
    public void AddHabitCompletion_ShouldNotAddDuplicateDates()
    {
        var habits = new Dictionary<string, HashSet<string>> {
            { "Meditation", new HashSet<string> { "2024-03-01 00:00:00" } }
        };

        HabitDictionaryHelper.AddHabitCompletion(ref habits, "Meditation", "2024-03-01 00:00:00");

        Assert.Single(habits["Meditation"]);
    }

    [Fact]
    public void AddHabitCompletion_ShouldHandleMultipleDifferentHabits()
    {
        var habits = new Dictionary<string, HashSet<string>>();

        HabitDictionaryHelper.AddHabitCompletion(ref habits, "Running", "2024-04-01 00:00:00");
        HabitDictionaryHelper.AddHabitCompletion(ref habits, "Yoga", "2024-04-02 00:00:00");

        Assert.Equal(2, habits.Count);
        Assert.Contains("Running", habits.Keys);
        Assert.Contains("Yoga", habits.Keys);
        Assert.Contains("2024-04-01 00:00:00", habits["Running"]);
        Assert.Contains("2024-04-02 00:00:00", habits["Yoga"]);
    }

    [Fact]
    public void AddHabitCompletion_ShouldHandleEmptyDictionary()
    {
        var habits = new Dictionary<string, HashSet<string>>();

        HabitDictionaryHelper.AddHabitCompletion(ref habits, "Journaling", "2024-05-01 00:00:00");

        Assert.Single(habits);
        Assert.Single(habits["Journaling"]);
        Assert.Contains("2024-05-01 00:00:00", habits["Journaling"]);
    }

    [Fact]
    public void ConvertToHabits_ValidDictionary_ReturnsExpectedHabits()
    {
        var inputDictionary = new Dictionary<string, HashSet<string>>
        {
            { "Exercise", new HashSet<string> { "2024-01-01 00:00:00", "2024-02-01 00:00:00" } },
            { "Reading", new HashSet<string> { "2024-03-01 00:00:00" } }
        };

        var expectedHabits = new List<Habit>
        {
            new Habit(0, "Exercise", new[] { new DateTime(2024, 1, 1), new DateTime(2024, 2, 1) }),
            new Habit(0, "Reading", new[] { new DateTime(2024, 3, 1) })
        };

        var result = HabitDictionaryHelper.ConvertToHabits(inputDictionary).ToList();

        Assert.Equal(expectedHabits.Count, result.Count);
        for (int i = 0; i < expectedHabits.Count; i++)
        {
            Assert.Equal(expectedHabits[i].Name, result[i].Name);
            Assert.Equal(expectedHabits[i].Completions, result[i].Completions);
        }
    }

    [Fact]
    public void ConvertToHabits_EmptyDictionary_ReturnsEmptyList()
    {
        var inputDictionary = new Dictionary<string, HashSet<string>>();

        var result = HabitDictionaryHelper.ConvertToHabits(inputDictionary).ToList();

        Assert.Empty(result);
    }
}
