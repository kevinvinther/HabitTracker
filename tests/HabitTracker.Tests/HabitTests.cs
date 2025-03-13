namespace HabitTracker.Tests
{
    public class HabitTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithName()
        {
            var habit = new Habit(1, "Exercise");

            Assert.Equal("Exercise", habit.Name);
        }

        [Theory]
        [InlineData("Exercise")]
        [InlineData("Read")]
        [InlineData("Meditate")]
        public void Constructor_ShouldAcceptDifferentNames(string name)
        {
            var habit = new Habit(1, name);
            Assert.Equal(name, habit.Name);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new Habit(1, ""));
            Assert.Equal("Name cannot be empty (Parameter 'name')", exception.Message);
        }

        [Fact]
        public void Constructor_ShouldAcceptCompletions()
        {
            var completion1 = new DateTime(2023, 12, 25);
            var completion2 = new DateTime(2023, 12, 31);

            var habit = new Habit(0, "Exercise", completion1, completion2);

            Assert.Equal(2, habit.Completions.Count);
            Assert.Contains(completion1, habit.Completions);
            Assert.Contains(completion2, habit.Completions);
        }

        [Fact]
        public void AddCompletion_ShouldAddNewCompletion()
        {
            var habit = new Habit(0, "Exercise");
            var newCompletion = new DateTime(2024, 1, 1);

            habit.AddCompletion(newCompletion);

            Assert.Single(habit.Completions);
            Assert.Contains(newCompletion, habit.Completions);
        }

        [Fact]
        public void RemoveCompletion_ShouldRemoveCompletion()
        {
            var habit = new Habit(0, "Exercise");
            var newCompletion = new DateTime(2024, 1, 1);

            habit.AddCompletion(newCompletion);
            habit.RemoveCompletion(newCompletion);

            Assert.Empty(habit.Completions);
        }

        [Fact]
        public void RemoveCompletion_ShouldReturnFalseOnBadRemoval()
        {
            // Bad removal in this case means the removal of an item not in the list.
            var habit = new Habit(0, "Exercise");
            var newCompletion = new DateTime(2024, 1, 1);


            Assert.False(habit.RemoveCompletion(newCompletion));
        }

        [Fact]
        public void RemoveCompletion_ShouldNotThrow_ForMissingCompletion()
        {
            var habit = new Habit(0, "Exercise");
            var completion = new DateTime(2024, 1, 1);

            habit.RemoveCompletion(completion);

            Assert.Empty(habit.Completions);
        }

        [Fact]
        public void ToString_ShouldConvert_WhenProperlyInitialized()
        {
            var habit = new Habit(0, "Exercise");

            Assert.Equal($"{habit}", habit.Name);
        }

        [Fact]
        public void PrintCompletionDates_ShouldPrint_AllCompletionDates()
        {
            List<DateTime> completions = new List<DateTime> {
                new DateTime(2024, 1, 1),
                new DateTime(2024, 1, 2),
                new DateTime(2024, 1, 3),
                new DateTime(2024, 1, 4)
            };

            var habit = new Habit(0, "Exercise", completions.ToArray());

            var expected = "Habit: Exercise\n* 2024-01-01 00:00:00\n* 2024-01-02 00:00:00\n* 2024-01-03 00:00:00\n* 2024-01-04 00:00:00\n";

            Assert.Equal(habit.GetCompletionDates(), expected);
        }

        [Fact]
        public void PrintCompletionDates_ShouldPrint_When_NoCompletions()
        {
            var habit = new Habit(0, "Exercise");

            var expected = "There are not yet any completions for habit Exercise! :(";
            Assert.Equal(habit.GetCompletionDates(), expected);
        }
    }
}
