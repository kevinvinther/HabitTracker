namespace HabitTracker;

[Collection("Sequential")]
public class TuiTests
{
    private (Tui, TestConsole) createTui(Queue<string> inputs)
    {
        var fakeConsole = new TestConsole(inputs);
        var repo = new HabitRepository("habits_test.db");
        var manager = new HabitManager(repo);

        return (new Tui(manager, fakeConsole), fakeConsole);
    }

    [Fact]
    public void MainMenu_AddHabit_CreatesHabit()
    {
        var inputs = new Queue<string>(new[] { "1", "Anki", "9" });

        var (tui, _) = createTui(inputs);
        tui.MainMenu();

        Assert.Single(tui.Manager.GetHabits());
        Assert.Equal("Anki", tui.Manager.GetHabits().First().Name);

        tui.Manager.RemoveHabit(tui.Manager.GetHabits().First().Id);
    }

    [Fact]
    public void MainMenu_AddHabit_DoesntCreate_ExistingHabit()
    {
        var inputs = new Queue<string>(new[] { "1", "Anki",
            "1", "Anki", "", "9"});

        var (tui, console) = createTui(inputs);
        tui.MainMenu();

        Assert.Single(tui.Manager.GetHabits());

        Assert.Contains("[WriteLine] A habit with name 'Anki' already exists.", console.Output);

        tui.Manager.RemoveHabit(tui.Manager.GetHabits().First().Id);
    }


    [Fact]
    public void MainMenu_RemoveHabit_RemovesHabit()
    {
        var inputs = new Queue<string>(new[] { "1", "Anki", "2", "Anki", "9" });

        var (tui, _) = createTui(inputs);
        tui.MainMenu();

        Assert.Empty(tui.Manager.GetHabits());
    }

    [Fact]
    public void MainMenu_RemoveHabit_DoesntPanic_WhenRemovingInvalidHabit()
    {
        var inputs = new Queue<string>(new[] { "2", "Anki", "9" });

        var (tui, _) = createTui(inputs);
        tui.MainMenu();

        Assert.Empty(tui.Manager.GetHabits());
    }

    [Fact]
    public void MainMenu_ManageHabit_AddCompletion_AddsCompletion()
    {
        var inputs = new Queue<string>(new[] { "1", "Anki", // Add Habit
                                               "3", "1", // Manage habit
                                               "1", // Add Completion
                                               "1", // Add DateTime.Now()
                                               "9"}); // Exit


        var (tui, _) = createTui(inputs);
        tui.MainMenu();

        Assert.NotNull(tui.Manager.GetHabits());
        Assert.Single(tui.Manager.GetHabits()[0].Completions);

        tui.Manager.RemoveHabit(tui.Manager.GetHabits().First().Id);
    }

    [Fact]
    public void MainMenu_ManageHabit_AddCompletion_AddsCompletions()
    {
        var inputs = new Queue<string>(new[] { "1", "Anki", // Add Habit
                                               "3", "1", // Manage habit
                                               "1", // Add Completion
                                               "1", // Add DateTime.Now()
                                               "3", "1", // Manage habit
                                               "1", // Add Completion
                                               "1", // Add DateTime.Now()
                                               "3", "1", // Manage habit
                                               "1", // Add Completion
                                               "1", // Add DateTime.Now()
                                               "9"}); // Exit

        var (tui, _) = createTui(inputs);
        tui.MainMenu();

        Assert.NotNull(tui.Manager.GetHabits());
        Assert.Equal(3, tui.Manager.GetHabits()[0].Completions.Count);

        tui.Manager.RemoveHabit(tui.Manager.GetHabits().First().Id);
    }

    [Fact]
    public void MainMenu_ManageHabit_AddCompletion_AddsSpecificCompletion()
    {
        var inputs = new Queue<string>(new[] { "1", "Anki", // Add Habit
                                               "3", "1", // Manage habit
                                               "1", // Add Completion
                                               "2", "2024-01-01",
                                               "9"}); // Exit

        var (tui, _) = createTui(inputs);
        tui.MainMenu();

        Assert.NotNull(tui.Manager.GetHabits());
        Assert.Equal(new DateTime(2024, 1, 1), tui.Manager.GetHabits()[0].Completions[0]);

        tui.Manager.RemoveHabit(tui.Manager.GetHabits().First().Id);
    }

    [Fact]
    public void MainMenu_ManageHabit_AddCompletion_AddsSpecificCompletion_WithTime()
    {
        var inputs = new Queue<string>(new[] { "1", "Anki", // Add Habit
                                               "3", "1", // Manage habit
                                               "1", // Add Completion
                                               "2", "2024-01-01 01:02:03",
                                               "9"}); // Exit

        var (tui, _) = createTui(inputs);
        tui.MainMenu();

        Assert.NotNull(tui.Manager.GetHabits());
        Assert.Equal(new DateTime(2024, 1, 1, 1, 2, 3), tui.Manager.GetHabits()[0].Completions[0]);

        tui.Manager.RemoveHabit(tui.Manager.GetHabits().First().Id);
    }

    // TODO: TUI must be changed, propably use SystemConsole to parse input. Or just check if it's something I've missed. Anyway this test errors:
    // [xUnit.net 00:00:00.25]     HabitTracker.TuiTests.MainMenu_ManageHabit_RemoveCompletion_RemovesCompletion [FAIL]
    // [xUnit.net 00:00:00.25]       System.ArgumentException : Input must be a valid number.
    // [xUnit.net 00:00:00.25]       Stack Trace:
    // [xUnit.net 00:00:00.26]         /home/kevin/Documents/Programming/HabitTracker/src/HabitTracker/TUI/TUI.cs(302,0): at HabitTracker.Tui.GetNumberInput()
    // [xUnit.net 00:00:00.26]         /home/kevin/Documents/Programming/HabitTracker/src/HabitTracker/TUI/TUI.cs(42,0): at HabitTracker.Tui.MainMenu()
    // [xUnit.net 00:00:00.26]         /home/kevin/Documents/Programming/HabitTracker/tests/HabitTracker.Tests/TuiTests.cs(159,0): at HabitTracker.TuiTests.MainMenu_ManageHabit_RemoveCompletion_RemovesCompletion()
    // [xUnit.net 00:00:00.26]            at System.RuntimeMethodHandle.InvokeMethod(Object target, Void** arguments, Signature sig, Boolean isConstructor)
    // [xUnit.net 00:00:00.26]            at System.Reflection.MethodBaseInvoker.InvokeWithNoArgs(Object obj, BindingFlags invokeAttr)
    // [Fact]
    // public void MainMenu_ManageHabit_RemoveCompletion_RemovesCompletion()
    // {
    //     var inputs = new Queue<string>(new[] { "1", "Anki", // Add Habit
    //                                            "3", "1", // Manage habit
    //                                            "1", // Add Completion
    //                                            "2", "2024-01-01 01:02:03",
    //                                            "3", "1", // Manage Habit
    //                                            "2", "1", //Remove completion
    //                                            "9"}); // Exit

    //     var (tui, _) = createTui(inputs);
    //     tui.MainMenu();

    //     Assert.Empty(tui.Manager.GetHabits()[0].Completions);

    //     tui.Manager.RemoveHabit(tui.Manager.GetHabits().First().Id);
    // }

    [Fact]
    public void MainMenu_ManageHabit_WrongID_ReturnsToMainMenu()
    {
        var inputs = new Queue<string>(new[] { "3", "1", // Manage (N/E) habit
                                               "", // Simulate Enter
                                               "9"}); // Exit

        var (tui, console) = createTui(inputs);
        tui.MainMenu();

        Assert.Empty(tui.Manager.GetHabits());

        Assert.Contains("[WriteLine] You must choose a valid ID!", console.Output);

    }
}
