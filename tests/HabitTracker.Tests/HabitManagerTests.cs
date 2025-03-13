using System.Globalization;

namespace HabitTracker.Tests
{
    [Collection("Sequential")]
    public class HabitManagerTests
    {
        private HabitManager CreateHabitManager()
        {
            var repository = new HabitRepository("habits_test.db");
            var manager = new HabitManager(repository);
            manager.InitializeDatabase();
            return manager;
        }

        [Fact]
        public void Constructor_ShouldInitializeWithEmptyDatabase()
        {
            HabitManager manager = CreateHabitManager();

            Assert.Empty(manager.GetHabits());
        }

        [Fact]
        public void AddHabit_ShouldAddNewHabit()
        {
            HabitManager manager = CreateHabitManager();

            Habit anki = new Habit(1, "anki");
            manager.AddHabit(anki);

            var habits = manager.GetHabits();
            Assert.Single(habits);
            Assert.Equal("anki", habits.First().Name);


            manager.RemoveHabit(anki.Id);
        }

        [Fact]
        public void AddHabit_ShouldThrow_WhenHabitExists()
        {
            HabitManager manager = CreateHabitManager();

            Habit anki = new Habit(1, "anki");

            manager.AddHabit(anki);

            var exception = Assert.Throws<InvalidOperationException>(() => manager.AddHabit(anki));
            Assert.Equal("A habit with name 'anki' already exists.", exception.Message);

            manager.RemoveHabit(anki.Id);
        }

        [Fact]
        public void RemoveHabit_ShouldRemoveExistingHabit()
        {
            HabitManager manager = CreateHabitManager();

            Habit anki = new Habit(1, "anki");
            manager.AddHabit(anki);

            manager.RemoveHabit(anki.Id);

            Assert.Empty(manager.GetHabits());
        }

        [Fact]
        public void RemoveHabit_ShouldThrow_WhenHabitDoesNotExist()
        {
            HabitManager manager = CreateHabitManager();

            Habit anki = new Habit(1, "anki");
            manager.AddHabit(anki);

            var nonExistentId = 999;
            var exception = Assert.Throws<InvalidOperationException>(() => manager.RemoveHabit(nonExistentId));
            Assert.Equal($"Habit with ID {nonExistentId} does not exist.", exception.Message);

            manager.RemoveHabit(anki.Id);
        }

        [Fact]
        public void AddCompletionTime_ShouldAddCompletion_ForHabit()
        {
            HabitManager manager = CreateHabitManager();

            Habit anki = new Habit(1, "anki");
            manager.AddHabit(anki);

            DateTime completionTime = new DateTime(2024, 1, 1, 10, 30, 31);
            manager.AddCompletion(anki.Id, completionTime);

            var habits = manager.GetHabits();
            var completions = habits.First().Completions;

            Assert.Single(completions);
            Assert.Contains(completionTime, completions);

            manager.RemoveHabit(anki.Id);
        }

        [Fact]
        public void AddCompletionTime_ShouldAddMultipleCompletions_ForHabit()
        {
            HabitManager manager = CreateHabitManager();

            Habit anki = new Habit(1, "anki");
            manager.AddHabit(anki);

            DateTime[] completionTimes =
            [
                new DateTime(2024, 1, 1, 10, 30, 31),
                 new DateTime(2024, 1, 1, 10, 30, 32),
                 new DateTime(2024, 1, 1, 10, 30, 33),
                 new DateTime(2024, 1, 1, 10, 30, 34)
            ];

            foreach (var time in completionTimes)
            {
                manager.AddCompletion(anki.Id, time);
            }

            var habits = manager.GetHabits();
            var completions = habits.First().Completions;

            Assert.Equal(completionTimes.Length, completions.Count);
            Assert.True(completionTimes.All(time => completions.Contains(time)));

            manager.RemoveHabit(anki.Id);
        }

        [Fact]
        public void RemoveCompletionTime_ShouldRemoveExistingCompletion()
        {
            HabitManager manager = CreateHabitManager();

            Habit anki = new Habit(1, "anki");
            manager.AddHabit(anki);

            DateTime completionTime = new DateTime(2024, 1, 1, 1, 1, 1);
            manager.AddCompletion(anki.Id, completionTime);

            manager.RemoveCompletion(anki.Id, completionTime);

            var habits = manager.GetHabits();
            var completions = habits.First().Completions;


            Assert.Empty(completions);

            manager.RemoveHabit(anki.Id);
        }

        [Fact]
        public void RemoveCompletionTime_ShouldThrow_WhenCompletionDoesntExist()
        {
            HabitManager manager = CreateHabitManager();

            Habit anki = new Habit(1, "anki");
            manager.AddHabit(anki);

            DateTime completionTime = new DateTime(2024, 1, 1, 1, 1, 1);

            string formattedDateTime = completionTime.ToString(
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture);  // Prevents OS-Specific variations to date format

            var exception = Assert.Throws<InvalidOperationException>(
                () => manager.RemoveCompletion(anki.Id, completionTime));
            Assert.Equal($"No completion found for Habit ID {anki.Id} at {formattedDateTime}", exception.Message);

            manager.RemoveHabit(anki.Id);
        }
    }
}
