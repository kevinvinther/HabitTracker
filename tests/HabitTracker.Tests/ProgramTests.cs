namespace HabitTracker.Tests;

[Collection("Sequential")]
public class ProgramTests
{
    private (HabitRepository, Tui, TestConsole) CreateTestProgram(Queue<string> inputs)
    {
        var fakeConsole = new TestConsole(inputs);
        var repo = new HabitRepository("habits_test.db");
        var manager = new HabitManager(repo);
        var tui = new Tui(manager, fakeConsole);

        return (repo, tui, fakeConsole);
    }

    [Fact]
    public void Program_RunApplication_AddsHabit()
    {
        var inputs = new Queue<string>(new[] { "1", "Anki", "9" }); // "1" to add, "Anki" as name, "9" to exit

        var (repo, tui, console) = CreateTestProgram(inputs);

        Program.RunApplication(repo, console);

        Assert.Single(tui.Manager.GetHabits());
        Assert.Equal("Anki", tui.Manager.GetHabits().First().Name);

        tui.Manager.RemoveHabit(tui.Manager.GetHabits().First().Id);
    }

    [Fact]
    public void Program_RunApplication_HandlesInvalidHabitGracefully()
    {
        var inputs = new Queue<string>(new[] { "2", "NonExistentHabit", "9" }); // "2" to remove habit, "9" to exit

        var (repo, tui, console) = CreateTestProgram(inputs);

        Program.RunApplication(repo, console);

        Assert.Empty(tui.Manager.GetHabits());

        Assert.Contains("[WriteLine] Could not find habit with name 'NonExistentHabit'!", console.Output);
    }

    [Fact]
    public void Main_ShouldRunWithoutErrors()
    {
        var input = new StringReader("9\n");
        Console.SetIn(input);

        var output = new StringWriter();
        Console.SetOut(output);

        Program.Main();

        var consoleOutput = output.ToString();

        Assert.Contains("1. Add new habit", consoleOutput); // Ensure main menu was printed
    }
}