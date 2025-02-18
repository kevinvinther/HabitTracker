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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("=======================");
                Console.WriteLine("=    HABIT TRACKER    =");
                Console.WriteLine("=======================");
                Console.ForegroundColor = ConsoleColor.White;
                if (_manager.GetHabits().Any())
                {
                    Console.WriteLine("Current habits:");
                    foreach (var habit in _manager.GetHabits())
                    {
                        string currentHabit = "= " + habit.Name.PadRight(20) + "=";

                        Console.WriteLine(currentHabit);
                    }
                }
                Console.WriteLine("Habits which have not been completed today:");
                foreach (var habit in GetHabitsNotCompletedToday(_manager.GetHabits()))
                {
                    Console.WriteLine($"* {habit.Name}");
                }
                Console.WriteLine("Please choose your option:");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("1. Add new habit");
                Console.WriteLine("2. Remove a habit");
                Console.WriteLine("3. Manage Habit");
                Console.WriteLine("9. Quit Program");
                Console.ForegroundColor = ConsoleColor.White;

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
                Habit newHabit = new Habit(0, name);
                _manager.AddHabit(newHabit);
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
                Habit? toBeDeleted = _manager.GetHabits().FirstOrDefault(h => h.Name.ToLower() == delete.ToLower());
                if (toBeDeleted != null)
                {
                    _manager.RemoveHabit(toBeDeleted.Id);
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
            foreach (var h in _manager.GetHabits())
            {
                i += 1;
                Console.WriteLine($"{i}: {h.Name}");
            }

            bool success = Int32.TryParse(Console.ReadLine(), out var index);

            if (!success)
            {
                Console.WriteLine("You must choose a number.");
                return;
            }

            var habitId = index - 1;

            Console.Clear();
            var habit = _manager.GetHabits()[habitId];
            habit.PrintCompletionDates();
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1. Add Completion");
            Console.WriteLine("2. Remove Completion Date");
            Console.WriteLine("9. Cancel");

            _ = Int32.TryParse(Console.ReadLine(), out index);

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
            PrintCompletionsWithIndex(habit);
            var completionIndex = GetNumberInput();
            if (completionIndex > habit.Completions.Count - 1 || completionIndex < 0)
            {
                throw new ArgumentException("You must choose a valid index!");
            }
            _manager.RemoveCompletion(habit.Id, habit.Completions[completionIndex]);
            Console.WriteLine($"Completion {habit.Completions[completionIndex]} deleted."); 
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Prints the completions of the habit in an indexed list format.
        /// </summary>
        /// <param name="habit">The habit whose completions will be printed.</param>
        private void PrintCompletionsWithIndex(Habit habit)
        {
            var i = 0;
            foreach (var completion in habit.Completions)
            {
                i += 1;
                Console.WriteLine($"{i}: {completion}");
            }
        }

        /// <summary>
        /// Get user input in the form of a number. If it is not a number, throw error.
        /// </summary>
        /// <returns>The number supplied by the user.</returns>
        /// <exception cref="ArgumentException">Throws ArgumentException error if the input is not a Int32 parseable number.</exception>
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
