namespace HabitTracker
{
    using System.Linq;

    public class Tui
    {
        private readonly HabitManager _manager = new();

        public void MainMenu()
        {
            _manager.InitializeDatabase();

            while (true)
            {
                Console.Clear();

                DisplayHeader();

                var habits = _manager.GetHabits().ToList();

                if (habits.Any())
                    DisplayHabits(habits);

                DisplayHabitsNotCompletedToday(habits);
                DisplayMenuOptions();

                int option = GetNumberInput();
                HandleMenuOption(option);
            }
        }


        public void DisplayHeader() {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("=======================");
            Console.WriteLine("=    HABIT TRACKER    =");
            Console.WriteLine("=======================");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void DisplayHabits(List<Habit> habits) {
            Console.WriteLine("Current habits:");
            foreach (var habit in habits)
            {
                Console.WriteLine($"= {habit.Name.PadRight(20)} =");
            }
        }

        public void DisplayMenuOptions() {
            Console.WriteLine("Please choose your option:");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("1. Add new habit");
            Console.WriteLine("2. Remove a habit");
            Console.WriteLine("3. Manage Habit");
            Console.WriteLine("9. Quit Program");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void DisplayHabitsNotCompletedToday(List<Habit> habits) {
            Console.WriteLine("Habits which have not been completed today:");
            PrintElementsWithIndex(GetHabitsNotCompletedToday(habits));
        }

        public bool HandleMenuOption(int option) {
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
                    return false;
                default:
                    Console.WriteLine("You must choose a valid option!");
                    break;
            }
            return true;
        }

        /// <summary>
        /// Wrapper to add habit in the TUI. Handles empty input.
        /// </summary>
        private void AddHabit()
        {
            Console.WriteLine("Please enter the name of your new habit: ");
            String name = GetStringInput();
            Habit newHabit = new Habit(0, name);
            _manager.AddHabit(newHabit);
        }

        /// <summary>
        /// Wrapper to remove habit in the TUI. Checks if habit exists using Linq.
        /// </summary>
        private void RemoveHabit()
        {
            Console.WriteLine("Please enter the name of the habit you want to remove: ");
            var delete = GetStringInput();
            Habit? toBeDeleted = _manager
                .GetHabits()
                .FirstOrDefault(h => h.Name.ToLower() == delete.ToLower());
            if (toBeDeleted != null)
            {
                _manager.RemoveHabit(toBeDeleted.Id);
            }
            else
            {
                Console.WriteLine($"Could not find habit with name '{delete}'!");
            }
        }

        /// <summary>
        /// Interface to add or remove completion from habit.
        /// </summary>
        private void ManageHabit()
        {
            Console.WriteLine("Please choose a habit: ");
            PrintElementsWithIndex(_manager.GetHabits());
            var habitId = GetNumberInput() - 1;
            Console.Clear();
            
            var habit = _manager.GetHabits()[habitId];
            habit.PrintCompletionDates();
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1. Add Completion");
            Console.WriteLine("2. Remove Completion Date");
            Console.WriteLine("9. Cancel");

            _ = Int32.TryParse(Console.ReadLine(), out var index);

            switch (index)
            {
                case 1:
                    _manager.AddCompletion(_manager.GetHabits()[habitId].Id, DateTime.Now);
                    break;
                case 2:
                    RemoveCompletion(habit);
                    break;
                default:
                    return;
            }
        }

        /// <summary>
        /// Removes a completion based on a list of completions outputted by the program.
        /// </summary>
        /// <param name="habit">The habit who has a completion to be deleted.</param>
        /// <exception cref="ArgumentException">If the input is not a valid index in the <see cref="Habit"/>'s completions.</exception>
        private void RemoveCompletion(Habit habit)
        {
            Console.WriteLine($"Habit: {habit.Name}. Please choose a completion you want to delete.");
            PrintElementsWithIndex(habit.Completions);
            var completionIndex = GetNumberInput();
            if (completionIndex > habit.Completions.Count || completionIndex < 0)
            {
                throw new ArgumentException("You must choose a valid index!");
            }
            completionIndex -= 1;
            _manager.RemoveCompletion(habit.Id, habit.Completions[completionIndex]);
            Console.WriteLine($"Completion {habit.Completions[completionIndex]} deleted."); 
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Prints elements of a list of elements with their index preceding.
        /// </summary>
        /// <param name="elements">The list containing the elements to be printed.</param>
        private void PrintElementsWithIndex<T>(List<T> elements)
        {
            var i = 0;
            foreach (var element in elements)
            {
                i += 1;
                Console.WriteLine($"{i}: {element}");
            }
        }

        /// <summary>
        /// Get user input in the form of a number. If it is not a number, throw error.
        /// </summary>
        /// <returns>The number supplied by the user.</returns>
        /// <exception cref="ArgumentException">Throws ArgumentException error if the input is not an Int32 parseable number.</exception>
        private int GetNumberInput()
        {
            Console.Write("Number: ");
            bool success = Int32.TryParse(Console.ReadLine(), out var index);
            if (success)
            {
                return index;
            }

            throw new ArgumentException("Input must be a valid number.");
        }

        /// <summary>
        /// Gets user input in the form of a string. Checks whether it is an empty string, and returns the string if not.
        /// </summary>
        /// <returns>The string inputted by the user.</returns>
        /// <exception cref="ArgumentException">If the string is empty.</exception>
        private string GetStringInput()
        {
            string? delete = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(delete))
            {
                return delete;
            }
            else
            {
                throw new ArgumentException("You must type a habit name.");
            }
        }

        /// <summary>
        /// Gets the habits which have not been completed today.
        /// </summary>
        /// <param name="habits">The habits to filter through.</param>
        /// <returns>The habits whose newest completion was not today.</returns>
        private List<Habit> GetHabitsNotCompletedToday(List<Habit> habits)
        {
            List<Habit> notCompletedToday = new List<Habit>();
            
            foreach (var habit in habits)
            {
                if (!habit.Completions.Any() || habit.Completions.Last().Date != DateTime.Today)
                {
                    notCompletedToday.Add(habit);
                }
            }

            return notCompletedToday;
        }
    }
}
