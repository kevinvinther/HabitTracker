namespace HabitTracker
{
    static class Program
    {
        static void Main()
        {
            HabitRepository repository = new HabitRepository();
            HabitManager manager = new HabitManager(repository);
            Tui tui = new Tui(manager);
            tui.MainMenu();
        }
    }
}

