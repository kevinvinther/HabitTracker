namespace HabitTracker.Tests
{
    [Collection("Sequential")]
    public class HabitManagerTests
    {
        private HabitManager CreateHabitManager()
        {
            var manager = new HabitManager("habits_test.db");
            manager.InitializeDatabase();
            return manager;
        }

        private long GetHabitId(List<Habit> habits, String name)
        {
            return habits.Where(h => h.Name == name)
                .Select(h => h.Id)
                .FirstOrDefault();
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


            var habitId = GetHabitId(habits, "anki");
            manager.RemoveHabit(habitId);
        }

        [Fact]
        public void AddHabit_ShouldThrow_WhenHabitExists()
        {
            HabitManager manager = CreateHabitManager();

            Habit anki = new Habit(1, "anki");

            manager.AddHabit(anki);
                
            var exception = Assert.Throws<InvalidOperationException>(() => manager.AddHabit(anki));
            Assert.Equal("A habit with name 'anki' already exists.", exception.Message);

            var habitId = GetHabitId(manager.GetHabits(), "anki");
            manager.RemoveHabit(habitId);
        }

        [Fact]
        public void RemoveHabit_ShouldRemoveExistingHabit()
        {
            /* HabitManager manager = CreateHabitManager(); */
            /**/
            /* Habit anki = new Habit(1, "anki"); */
            /* manager.AddHabit(anki); */
            /**/
            /* manager.RemoveHabit(anki.Id); */
            /**/
            /* Assert.Empty(manager.GetHabits()); */
        }

        [Fact]
        public void RemoveHabit_ShouldThrow_WhenHabitDoesNotExist()
        {
            /* HabitManager manager = CreateHabitManager(); */
            /**/
            /* Habit anki = new Habit(1, "anki"); */
            /* manager.AddHabit(anki); */
            /**/
            /* var nonExistentId = 999; */
            /* var exception = Assert.Throws<InvalidOperationException>(() => manager.RemoveHabit(nonExistentId)); */
            /* Assert.Equal("Habit doesn't exist in directory!", exception.Message); */
        }

        [Fact]
        public void AddCompletionTime_ShouldAddCompletion_ForHabit()
        {
            /* HabitManager manager = CreateHabitManager(); */
            /**/
            /* Habit anki = new Habit(1, "anki"); */
            /* manager.AddHabit(anki); */
            /**/
            /* DateTime completionTime = new DateTime(2024, 1, 1, 10, 30, 31); */
            /* manager.AddCompletion(anki.Id, completionTime); */
            /**/
            /* var habits = manager.GetHabits(); */
            /* var completions = habits.First().CompletionTimes; */
            /**/
            /* Assert.Single(completions); */
            /* Assert.Contains(completionTime, completions); */
        }

        [Fact]
        public void AddCompletionTime_ShouldAddMultipleCompletions_ForHabit()
        {
            /* HabitManager manager = CreateHabitManager(); */
            /**/
            /* Habit anki = new Habit(1, "anki"); */
            /* manager.AddHabit(anki); */
            /**/
            /* DateTime[] completionTimes = new[] */
            /* { */
            /*     new DateTime(2024, 1, 1, 10, 30, 31), */
            /*     new DateTime(2024, 1, 1, 10, 30, 32), */
            /*     new DateTime(2024, 1, 1, 10, 30, 33), */
            /*     new DateTime(2024, 1, 1, 10, 30, 34) */
            /* }; */
            /**/
            /* foreach (var time in completionTimes) */
            /* { */
            /*     manager.AddCompletion(anki.Id, time); */
            /* } */
            /**/
            /* var habits = manager.GetHabits(); */
            /* var completions = habits.First().Completions; */
            /**/
            /* Assert.Equal(completionTimes.Length, completions.Count); */
            /* Assert.True(completionTimes.All(time => completions.Contains(time))); */
        }

        [Fact]
        public void RemoveCompletionTime_ShouldRemoveExistingCompletion()
        {
            /* HabitManager manager = CreateHabitManager(); */
            /**/
            /* Habit anki = new Habit(1, "anki"); */
            /* manager.AddHabit(anki); */
            /**/
            /* DateTime completionTime = new DateTime(2024, 1, 1, 1, 1, 1); */
            /* manager.AddCompletion(anki.Id, completionTime); */
            /**/
            /* manager.RemoveCompletion(anki.Id, completionTime); */
            /**/
            /* var habits = manager.GetHabits(); */
            /* var completions = habits.First().CompletionTimes; */
            /**/
            /* Assert.Empty(completions); */
        }

        [Fact]
        public void RemoveCompletionTime_ShouldReturnFalse_WhenCompletionDoesNotExist()
        {
            /* HabitManager manager = CreateHabitManager(); */
            /**/
            /* Habit anki = new Habit(1, "anki"); */
            /* manager.AddHabit(anki); */
            /**/
            /* DateTime completionTime = new DateTime(2024, 1, 1, 1, 1, 1); */
            /**/
            /* bool result = manager.RemoveCompletion(anki.Id, completionTime); */
            /**/
            /* Assert.False(result); */
        }
    }
}
