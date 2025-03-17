namespace HabitTracker
{
    static class Program
    {
        static void Main()
        {
            HabitRepository repository = new HabitRepository();
            HabitManager manager = new HabitManager(repository);
            IConsole console = new SystemConsole();
            Tui tui = new Tui(manager, console);
            tui.MainMenu();
        }
    }
}

