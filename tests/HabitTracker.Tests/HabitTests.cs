using Xunit;

namespace HabitTracker.Tests
{
    public class HabitTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithName()
        {
            // Arrange
            var habit = new Habit("Exercise");

            // Act & Assert
            Assert.Equal("Exercise", habit.Name);
        }

        [Theory]
        [InlineData("Exercise")]
        [InlineData("Read")]
        [InlineData("Meditate")]
        public void Constructor_ShouldAcceptDifferentNames(string name)
        {
            var habit = new Habit(name);
            Assert.Equal(name, habit.Name);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new Habit(""));
            Assert.Equal("Name cannot be empty (Parameter 'name')", exception.Message);
        }

        [Fact]
        public void Constructor_ShouldAcceptCompletions()
        {
            var completion1 = new DateTime(2023, 12, 25);
            var completion2 = new DateTime(2023, 12, 31);

            var habit = new Habit("Exercise", completion1, completion2);

            Assert.Equal(2, habit.Completions.Count);
            Assert.Contains(completion1, habit.Completions);
            Assert.Contains(completion2, habit.Completions);
        }

        [Fact]
        public void AddCompletion_ShouldAddNewCompletion()
        {
            // Arrange
            var habit = new Habit("Exercise");
            var newCompletion = new DateTime(2024, 1, 1);

            // Act
            habit.AddCompletion(newCompletion);

            // Assert
            Assert.Single(habit.Completions);
            Assert.Contains(newCompletion, habit.Completions);
        }
    }
}
