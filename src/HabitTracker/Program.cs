namespace HabitTracker
{
    public static class Program
    {
        public static void RunApplication(IHabitRepository repository, IConsole console)
        {
            HabitManager manager = new HabitManager(repository);
            Tui tui = new Tui(manager, console);
            tui.MainMenu();
        }

        public static void Main()
        {
            IHabitRepository repository = new HabitRepository();
            IConsole console = new SystemConsole();
            RunApplication(repository, console);
        }
    }
}