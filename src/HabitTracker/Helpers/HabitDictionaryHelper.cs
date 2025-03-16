namespace HabitTracker;

public static class HabitDictionaryHelper
{
    public static void AddHabitCompletion(ref Dictionary<string, HashSet<string>> dictionary, string key, string value)
    {
        if (!dictionary.TryGetValue(key, out var hashSet))
        {
            hashSet = new HashSet<string>();
            dictionary[key] = hashSet;
        }
        hashSet.Add(value);
    }

    public static IEnumerable<Habit> ConvertToHabits(Dictionary<string, HashSet<string>> dictionary)
    {
        return dictionary.Select(habit =>
            new Habit(0, habit.Key, DateTimeHelper.ParseDateStrings(habit.Value).ToArray())
        );
    }
}
