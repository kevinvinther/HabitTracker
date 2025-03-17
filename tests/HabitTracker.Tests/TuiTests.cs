namespace HabitTracker;

[Collection("Sequential")]
public class TuiTests
{
    private Tui createTui(Queue<string> inputs)
    {
        var fakeConsole = new TestConsole(inputs);
        var repo = new HabitRepository("habits_test.db");
        var manager = new HabitManager(repo);

        return new Tui(manager, fakeConsole);
    }

    [Fact]
    public void MainMenu_AddHabit_CreatesHabit()
    {
        var inputs = new Queue<string>(new[] { "1", "Anki", "9" });

        var tui = createTui(inputs);
        tui.MainMenu();

        Assert.Single(tui.Manager.GetHabits());
        Assert.Equal("Anki", tui.Manager.GetHabits().First().Name);

        tui.Manager.RemoveHabit(tui.Manager.GetHabits().First().Id);
    }

}
