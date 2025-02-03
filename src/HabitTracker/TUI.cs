namespace HabitTracker
{
    using System.Linq;

    public class TUI
    {
        public HabitManager Manager;

        public TUI()
        {
            Manager = new HabitManager();
            Habit anki = new Habit("Anki");
            Habit exercise = new Habit("Exercise");
            Habit coding = new Habit("Coding");
            Habit walk = new Habit("Walk");
            Manager.AddHabits(anki, exercise, coding, walk);
        }

        public void MainMenu()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("=======================");
                Console.WriteLine("=    HABIT TRACKER    =");
                Console.WriteLine("=======================");
                Console.ForegroundColor = ConsoleColor.White;
                if (Manager.Habits.Any())
                {
                    Console.WriteLine("Current habits:");
                    foreach (var habit in Manager.Habits)
                    {
                        string currentHabit = "= " + habit.Name.PadRight(20) + "=";

                        Console.WriteLine(currentHabit);
                    }
                }
                Console.WriteLine("Please choose your option:");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("1. Add new habit");
                Console.WriteLine("2. Remove a habit");

                string? answer = Console.ReadLine();

                if (int.TryParse(answer, out int option))
                {
                    switch (option)
                    {
                        case 1:
                            Console.WriteLine("Please enter the name of your new habit: ");
                            string? name = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(name))
                            {
                                Habit newHabit = new Habit(name);
                                Manager.AddHabits(newHabit);
                            }
                            else
                            {
                                Console.WriteLine("Habit name cannot be empty or null.");
                            }
                            break;
                        case 2:
                            Console.WriteLine("Please enter the name of the habit you want to remove: ");
                            string? delete = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(delete))
                            {
                                Habit? toBeDeleted = Manager.Habits.FirstOrDefault(h => h.Name == delete);
                                if (toBeDeleted != null)
                                {
                                    Manager.RemoveHabits(toBeDeleted);
                                }
                                else
                                {
                                    Console.WriteLine($"Could not find habit with name '{delete}'!");
                                }
                            }
                            break;
                        default:
                            Console.WriteLine("You must choose a valid option!");
                            break;

                    }
                }
                else
                {
                    Console.WriteLine("Input cannot be empty and must be a number.");
                }
            }
        }
    }
}
