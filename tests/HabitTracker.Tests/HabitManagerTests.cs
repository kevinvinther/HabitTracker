using Xunit;

namespace HabitTracker.Tests
{
    public class HabitManagerTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithCorrectHabits()
        {
            Habit anki = new Habit("Anki");
            Habit exercise = new Habit("Exercise");

            HabitManager manager = new HabitManager(anki, exercise);
            List<Habit> habits = new List<Habit> { anki, exercise };

            Assert.Equal(manager.Habits, habits);
        }

        [Fact]
        public void Constructor_ShouldInitializeEmpty()
        {
            HabitManager manager = new HabitManager();

            Assert.Empty(manager.Habits);
        }

        [Fact]
        public void AddHabits_AddsNewHabitsWithMultipleArguments()
        {
            HabitManager manager = new HabitManager();

            Habit anki = new Habit("Anki");
            Habit exercise = new Habit("Exercise");

            manager.AddHabits(anki, exercise);
            List<Habit> habits = new List<Habit> { anki, exercise };

            Assert.Equal(manager.Habits, habits);
        }

        [Fact]
        public void AddHabits_AddsNewHabitsWithOneArgument()
        {
            HabitManager manager = new HabitManager();

            Habit anki = new Habit("Anki");
            Habit exercise = new Habit("Exercise");

            manager.AddHabits(anki);
            manager.AddHabits(exercise);
            List<Habit> habits = new List<Habit> { anki, exercise };

            Assert.Equal(manager.Habits, habits);
        }

        [Fact]
        public void AddHabits_ShouldThrow_WhenHabitExists()
        {
            HabitManager manager = new HabitManager();

            Habit anki = new Habit("Anki");
            manager.AddHabits(anki);

            var exception = Assert.Throws<InvalidOperationException>(() => manager.AddHabits(anki));
            Assert.Equal("A Habit with name 'Anki' already exists.", exception.Message);
        }

        [Fact]
        public void RemoveHabits_ShouldRemoveHabitsWithMultipleArguments()
        {
            HabitManager manager = new HabitManager();

            Habit anki = new Habit("Anki");
            Habit exercise = new Habit("Exercise");

            manager.AddHabits(anki, exercise);

            manager.RemoveHabits(anki, exercise);

            Assert.Empty(manager.Habits);
        }

        [Fact]
        public void RemoveHabits_ShouldRemoveHabitsWithOneArgument()
        {
            HabitManager manager = new HabitManager();

            Habit anki = new Habit("Anki");
            Habit exercise = new Habit("Exercise");

            manager.AddHabits(anki, exercise);

            manager.RemoveHabits(anki);

            List<Habit> habits = new List<Habit> { exercise };

            Assert.Equal(manager.Habits, habits);
        }


        [Fact]
        public void RemoveHabits_ShouldThrow_WhenNoHabitExists()
        {
            HabitManager manager = new HabitManager();

            Habit anki = new Habit("Anki");
            Habit exercise = new Habit("Exercise");

            manager.AddHabits(anki);

            var exception = Assert.Throws<InvalidOperationException>(() => manager.RemoveHabits(exercise));
            Assert.Equal("Habit doesn't exist in directory!", exception.Message);
        }

        [Fact]
        public void AddCompletionTime_ShouldAddCompletion_WithOneTime()
        {
            HabitManager manager = new HabitManager();

            Habit anki = new Habit("Anki");
            manager.AddHabits(anki);

            Assert.Empty(manager.Habits[0].Completions);

            DateTime dt = new DateTime(2024, 1, 1, 10, 30, 31);
            manager.AddCompletionTime(anki, dt);

            List<DateTime> times = new List<DateTime> { dt };

            Assert.Equal(manager.Habits[0].Completions, times);
        }

        [Fact]
        public void AddCompletionTime_ShouldAddCompletion_WithMultipleTimes()
        {
            HabitManager manager = new HabitManager();

            Habit anki = new Habit("Anki");
            manager.AddHabits(anki);

            Assert.Empty(manager.Habits[0].Completions);

            DateTime[] dateTimes = [
                new DateTime(2024, 1, 1, 10, 30, 31),
                new DateTime(2024, 1, 1, 10, 30, 32),
                new DateTime(2024, 1, 1, 10, 30, 33),
                new DateTime(2024, 1, 1, 10, 30, 34)
            ];
            manager.AddCompletionTime(anki, dateTimes);

            Assert.Equal(manager.Habits[0].Completions, dateTimes);
        }

        [Fact]
        public void RemoveCompletionTime_ShouldRemoveCompletion_WithNormalInput()
        {
            HabitManager manager = new HabitManager();

            Habit anki = new Habit("Anki");
            manager.AddHabits(anki);

            DateTime dt = new DateTime(2024, 1, 1, 1, 1, 1);
            manager.AddCompletionTime(anki, dt);

            manager.RemoveCompletionTime(anki, dt);

            Assert.Empty(manager.Habits[0].Completions);

        }

        [Fact]
        public void RemoveCompletionTime_ShouldReturnFalse_WhenRemovingBadValue()
        {
            HabitManager manager = new HabitManager();

            Habit anki = new Habit("Anki");
            manager.AddHabits(anki);

            DateTime dt = new DateTime(2024, 1, 1, 1, 1, 1);

            Assert.False(manager.RemoveCompletionTime(anki, dt));
        }
    }
}
