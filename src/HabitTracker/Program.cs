namespace HabitTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Habit habit = new Habit("Anki");
            habit.AddCompletion(DateTime.Now);
            habit.AddCompletion(DateTime.Today);
            habit.AddCompletion(DateTime.Today);
            habit.AddCompletion(DateTime.Today);
            habit.PrintCompletionDates();
        }
    }
}

