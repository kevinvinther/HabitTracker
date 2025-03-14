namespace HabitTracker.Tests
{
    public class DateTimeHelperTests
    {
        // public static string Format(DateTime dt) =>
        //     dt.ToString(DateFormat, CultureInfo.InvariantCulture);

        // public static DateTime Parse(String dt) =>
        //     DateTime.ParseExact(dt, DateFormat, CultureInfo.InvariantCulture);

        [Fact]
        public void Format_ShouldFormat_WithNormalDate()
        {
            var date = new DateTime(2024, 1, 1, 16, 25, 0);

            Assert.Equal("2024-01-01 16:25:00", DateTimeHelper.Format(date));
        }

        [Fact]
        public void Format_ShouldFormat_WithoutTime()
        {
            var date = new DateTime(2024, 1, 1);

            Assert.Equal("2024-01-01 00:00:00", DateTimeHelper.Format(date));
        }

        [Fact]
        public void Parse_ShouldParse_WithNormalDate()
        {
            var date = "2024-01-01 22:38:09";
            var expected = new DateTime(2024, 1, 1, 22, 38, 9);

            Assert.Equal(expected, DateTimeHelper.Parse(date));
        }
    }
}
