namespace HabitTracker.Tests;

[Collection("Sequential")]
public class TuiTests
{
    private (Tui, TestConsole) CreateTui(Queue<string> inputs)
    {
        var fakeConsole = new TestConsole(inputs);
        var repo = new HabitRepository("habits_test.db");
        var manager = new HabitManager(repo);

        return (new Tui(manager, fakeConsole), fakeConsole);
    }

    private void RemoveHabits(HabitManager manager)
    {
        foreach (var habit in manager.GetHabits())
        {
            manager.RemoveHabit(habit.Id);
        }
    }

    [Fact]
    public void MainMenu_AddHabit_CreatesHabit()
    {
        var inputs = new Queue<string>(new[] { "1", "Anki", "9" });

        var (tui, _) = CreateTui(inputs);
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

        var (tui, console) = CreateTui(inputs);
        tui.MainMenu();

        Assert.Single(tui.Manager.GetHabits());

        Assert.Contains("[WriteLine] A habit with name 'Anki' already exists.", console.Output);

        tui.Manager.RemoveHabit(tui.Manager.GetHabits().First().Id);
    }


    [Fact]
    public void MainMenu_RemoveHabit_RemovesHabit()
    {
        var inputs = new Queue<string>(new[] { "1", "Anki", "2", "Anki", "9" });

        var (tui, _) = CreateTui(inputs);
        tui.MainMenu();

        Assert.Empty(tui.Manager.GetHabits());
    }

    [Fact]
    public void MainMenu_RemoveHabit_DoesntPanic_WhenRemovingInvalidHabit()
    {
        var inputs = new Queue<string>(new[] { "2", "Anki", "9" });

        var (tui, _) = CreateTui(inputs);
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


        var (tui, _) = CreateTui(inputs);
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

        var (tui, _) = CreateTui(inputs);
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

        var (tui, _) = CreateTui(inputs);
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

        var (tui, _) = CreateTui(inputs);
        tui.MainMenu();

        Assert.NotNull(tui.Manager.GetHabits());
        Assert.Equal(new DateTime(2024, 1, 1, 1, 2, 3), tui.Manager.GetHabits()[0].Completions[0]);

        tui.Manager.RemoveHabit(tui.Manager.GetHabits().First().Id);
    }

    [Fact]
    public void MainMenu_ManageHabit_RemoveCompletion_RemovesCompletion()
    {
        var inputs = new Queue<string>(new[] { "1", "Anki", // Add Habit
                                               "3", "1", // Manage habit
                                               "1", // Add Completion
                                               "2", "2024-01-01 01:02:03",
                                               "3", "1", // Manage Hab"Anki", it
                                               "2", "1", "", //Remove completion
                                               "9"}); // Exit

        var (tui, _) = CreateTui(inputs);
        tui.MainMenu();

        Assert.Empty(tui.Manager.GetHabits()[0].Completions);

        foreach (var habit in tui.Manager.GetHabits())
        {
            tui.Manager.RemoveHabit(habit.Id);
        }
    }

    [Fact]
    public void MainMenu_ManageHabit_WrongID_ReturnsToMainMenu()
    {
        var inputs = new Queue<string>(new[] { "3", "1", // Manage (N/E) habit
                                               "", // Simulate Enter
                                               "9"}); // Exit

        var (tui, console) = CreateTui(inputs);
        tui.MainMenu();

        Assert.Empty(tui.Manager.GetHabits());

        Assert.Contains("[WriteLine] You must choose a valid ID!", console.Output);
    }

    [Fact]
    public void MainMenu_ParsingNumber_HandlesFail()
    {
        var inputs = new Queue<string>(new[]
        {
            "hi", // Wrong input
            "", // Accept error message
            "9" // Quit program
        });

        var (tui, console) = CreateTui(inputs);
        tui.MainMenu();

        Assert.Contains("[WriteLine] Input must be a valid number.", console.Output);
    }

    [Fact]
    public void MainMenu_RemoveHabit_HandlesFail()
    {
        var inputs = new Queue<string>(new[]
        {
            "1", "Anki", // Create new habit
            "3", "1", // Manage habit
            "1", // Add Completion
            "2", "2024-01-01 01:02:03",
            "3", "1", // Manage habit
            "2", // Remove Completion
            "-1", "", // Non-existent negative completion
            "3", "1", // Manage habit
            "2", // Remove Completion
            "2", "", // Non-existent positive completion
            "9" // Quit
        });

        var (tui, console) = CreateTui(inputs);
        tui.MainMenu();

        Assert.Contains("[WriteLine] You must choose a valid index!", console.Output);

        RemoveHabits(tui.Manager);
    }

    [Fact]
    public void MainMenu_ManageHabit_HandlesWrongInput()
    {
        var inputs = new Queue<string>(new[]
        {
            "1", "Anki", // Create new habit
            "3", "1", // Manage habit
            "9", // Add Completion
            "9" // Quit
        });

        var (tui, _) = CreateTui(inputs);
        tui.MainMenu();

        RemoveHabits(tui.Manager);
    }

    [Fact]
    public void MainMenu_AddCompletion_Returns_OnWrongIndex()
    {
        var inputs = new Queue<string>(new[]
        {
            "1", "Anki", // Create new habit
            "3", "1", // Manage habit
            "1", // Add Completion
            "10000", // Wrong index
            "9" // Quit
        });

        var (tui, _) = CreateTui(inputs);
        tui.MainMenu();

        RemoveHabits(tui.Manager);
    }

    [Fact]
    public void MainMenu_AddOldCompletion_Handles_UnparsableDate()
    {
        var inputs = new Queue<string>(new[] { "1", "Anki", // Add Habit
                                               "3", "1", // Manage habit
                                               "1", // Add Completion
                                               "2", "2024-01-01 00:02", "", // Unparsable date
                                               "9"}); // Exit
        var (tui, console) = CreateTui(inputs);
        tui.MainMenu();

        Assert.Contains("[WriteLine] Could not parse date! Press any key to continue.", console.Output);

        RemoveHabits(tui.Manager);
    }

    [Fact]
    public void MainMenu_GetStringInput_HandlesEmptyInput()
    {
        var inputs = new Queue<string>(new[] { "1", "", // Add empty Habit
                                               "", // Accept message
                                               "9"}); // Exit

        var (tui, console) = CreateTui(inputs);
        tui.MainMenu();

        Assert.Contains("[WriteLine] You must type a habit name.", console.Output);
    }

    [Fact]
    public void MainMenu_GetMaxStreaks_GetsCorrectStreaks()
    {
        var inputs = new Queue<string>(new[] { "1", "Anki", // Add Habit
                                               "3", "1", // Manage habit
                                               "1", // Add Completion
                                               "2", "2024-01-01", // Add date 1
                                               "3", "1", // Manage habit
                                               "1", // Add Completion
                                               "2", "2024-01-02", // Add date 2
                                               "4", "", // Get max streaks and accept
                                               "9"}); // Exit

        var (tui, console) = CreateTui(inputs);
        tui.MainMenu();

        Assert.Contains("[WriteLine] Anki: 2", console.Output);

        RemoveHabits(tui.Manager);
    }

    [Fact]
    public void MainMenu_GetCurrentStreaks_GetsCorrectStreaks()
    {
        var inputs = new Queue<string>(new[] { "1", "Anki", // Add Habit
                                               "3", "1", // Manage habit
                                               "1", // Add Completion
                                               "2", "2024-01-01", // Add date 1
                                               "3", "1", // Manage habit
                                               "1", // Add Completion
                                               "2", "2024-01-02", // Add date 2
                                               "3", "1", // Manage habit
                                               "1", // Add Completion
                                               "1", // Add date now
                                               "5", "", // Get current streaks and accept
                                               "9"}); // Exit

        var (tui, console) = CreateTui(inputs);
        tui.MainMenu();

        Assert.Contains("[WriteLine] Anki: 1", console.Output);

        RemoveHabits(tui.Manager);
    }

    [Fact]
    public void MainMenu_Importer_HandlesWrongInput()
    {
        var inputs = new Queue<string>(new[] {
                                               "6", "", // Write Empty 
                                               "9"}); // Exit

        var (tui, _) = CreateTui(inputs);
        tui.MainMenu();

        RemoveHabits(tui.Manager);
    }

    [Fact]
    public void MainMenu_Importer_HandlesNonexistentFile()
    {
        var inputs = new Queue<string>(new[] {
            "6", "hello.csv", // Write non-existent file
            "9"}); // Exit

        var (tui, console) = CreateTui(inputs);
        tui.MainMenu();

        Assert.NotEmpty(
            console.Output.Find(
                c => c.StartsWith("[WriteLine] Import failed. Error: Could not find file")) ?? throw new InvalidOperationException());

        RemoveHabits(tui.Manager);
    }

    const string ValidCsv = "../../../TestData/Habitica1.csv";

    [Fact]
    public void MainMenu_Importer_HandlesWorkingFile()
    {
        var inputs = new Queue<string>(new[] {
            "6", ValidCsv, // Write valid file
            "9"}); // Exit

        var (tui, console) = CreateTui(inputs);
        tui.MainMenu();
        Assert.Contains("[WriteLine] = Se 1 forelæsning     =", console.Output);
        Assert.Contains("[WriteLine] = Lav 1 lektion værd af opgaver =", console.Output);
        Assert.Contains("[WriteLine] = Læs 1 kapitel i en bog =", console.Output);
        Assert.Contains("[WriteLine] = Anki                 =", console.Output);
        Assert.Contains("[WriteLine] = Allergivaccine       =", console.Output);

        RemoveHabits(tui.Manager);
    }
}
