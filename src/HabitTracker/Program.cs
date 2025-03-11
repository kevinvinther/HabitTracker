namespace HabitTracker
{
    static class Program
    {
        static void Main()
        {
            HabitManager manager = new HabitManager();
            Tui tui = new Tui(manager);
            tui.MainMenu();
        }
    }
}

