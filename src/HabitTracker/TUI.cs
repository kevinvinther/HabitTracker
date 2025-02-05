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
                Console.Clear();
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
                Console.WriteLine("3. Manage Habit");
                Console.WriteLine("9. Quit Program");

                string? answer = Console.ReadLine();

                if (int.TryParse(answer, out int option))
                {
                    switch (option)
                    {
                        case 1:
                            AddHabit();
                            break;
                        case 2:
                            RemoveHabit();
                            break;
                        case 3:
                            ManageHabit();
                            break;
                        case 9:
                            return;
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

        /// <summary>
        /// Wrapper to add habit in the TUI. Handles empty input.
        /// </summary>
        private void AddHabit()
        {
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
        }

        /// <summary>
        /// Wrapper to remove habit in the TUI. Checks if habit exists using Linq.
        /// </summary>
        private void RemoveHabit()
        {
            Console.WriteLine("Please enter the name of the habit you want to remove: ");
            string? delete = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(delete))
            {
                Habit? toBeDeleted = Manager.Habits.FirstOrDefault(h => h.Name.ToLower() == delete.ToLower());
                if (toBeDeleted != null)
                {
                    Manager.RemoveHabits(toBeDeleted);
                }
                else
                {
                    Console.WriteLine($"Could not find habit with name '{delete}'!");
                }
            }
        }

        /// <summary>
        /// Add or remove completion from habit.
        /// </summary>
        private void ManageHabit()
        {
            Console.WriteLine("Please choose a habit: ");
            var i = 0;
            foreach (var h in Manager.Habits)
            {
                i += 1;
                Console.WriteLine($"{i}: {h.Name}");
            }

            int index;
            bool success = Int32.TryParse(Console.ReadLine(), out index);

            if (!success)
            {
                Console.WriteLine("You must choose a number.");
                return;
            }

            index -= 1; // Arrays are 0-indexed, but we present them as 1-indexed

            Console.Clear();
            var habit = Manager.Habits[index];
            habit.PrintCompletionDates();
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1. Add Completion");
            Console.WriteLine("2. Remove Completion Date");

            success = Int32.TryParse(Console.ReadLine(), out index);

            switch (index)
            {
                case 1:
                    habit.AddCompletion(DateTime.Now);
                    break;
                case 2:
                    RemoveCompletion(habit);
                    break;
                default:
                    Console.WriteLine("You must choose a number.");
                    return;
            }
        }

        /// <summary>
        /// Gives a guide to remove a completion.
        /// </summary>
        /// <param name="habit">The habit which a completion is to be removed from.</param>
        private void RemoveCompletion(Habit habit)
        {
            Console.WriteLine($"Habit: {habit.Name}. Please choose a completion you want to delete: ");

            var i = 0;
            foreach (var completion in habit.Completions)
            {
                i += 1;
                Console.WriteLine($"{i}: {completion}");
            }


            Console.Write("Number: ");

            int index;
            bool success = Int32.TryParse(Console.ReadLine(), out index);

            if (!success)
            {
                Console.WriteLine("You must write a valid number.");
                return;
            }

            habit.RemoveCompletion(habit.Completions[index]);

            Console.WriteLine($"Completion {habit.Completions} deleted.");
            Console.WriteLine($"Press any key to continue");

            Console.ReadKey();
        }
    }
}
