namespace HabitTracker
{
    using System.Linq;

    public class Tui
    {
        private readonly HabitManager _manager;
        private IConsole _console;

        /// <summary>
        /// Sets up the variables to their given value.
        /// </summary>
        /// <param name="manager">The HabitManager instance to be used</param>
        public Tui(HabitManager manager, IConsole console)
        {
            _console = console;
            _manager = manager;
        }

        /// <summary>
        /// Handles the main loop as well as initializatio of the database
        /// for the TUI.
        /// </summary>
        public void MainMenu()
        {
            _manager.InitializeDatabase();

            while (true)
            {
                _console.Clear();

                DisplayHeader();

                var habits = _manager.GetHabits().ToList();

                if (habits.Any())
                    DisplayHabits(habits);

                DisplayHabitsNotCompletedToday(habits);
                DisplayMenuOptions();

                int option = GetNumberInput();
                _console.Clear();
                if (!HandleMenuOption(option))
                    return;
            }
        }


        /// <summary>
        /// Displays the header logo.
        /// </summary>
        private void DisplayHeader()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            _console.WriteLine("=======================");
            _console.WriteLine("=    HABIT TRACKER    =");
            _console.WriteLine("=======================");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Displays the habits in a neatly formatted list.
        /// </summary>
        /// <param name="habits">A list of habits.</param>
        private void DisplayHabits(List<Habit> habits)
        {
            _console.WriteLine("Current habits:");
            foreach (var habit in habits)
            {
                _console.WriteLine($"= {habit.Name.PadRight(20)} =");
            }
        }

        /// <summary>
        /// Display the menu options.
        /// </summary>
        private void DisplayMenuOptions()
        {
            _console.WriteLine("Please choose your option:");
            Console.ForegroundColor = ConsoleColor.Blue;
            _console.WriteLine("1. Add new habit");
            _console.WriteLine("2. Remove a habit");
            _console.WriteLine("3. Manage Habit");
            _console.WriteLine("4. Get Max Streaks");
            _console.WriteLine("5. Get Current Streaks");
            _console.WriteLine("6. Import Data form Other Habit Tracker");
            _console.WriteLine("9. Quit Program");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Displays the habits not completed today.
        /// </summary>
        /// <param name="habits">A list of habits.</param>
        private void DisplayHabitsNotCompletedToday(List<Habit> habits)
        {
            _console.WriteLine("Habits which have not been completed today:");
            PrintElementsWithIndex(_manager.GetHabitsNotCompletedOnDay(DateTime.Today));
        }

        /// <summary>
        /// Given an option, handles the menu options printed in DisplayMenuOptions.
        /// </summary>
        /// <param name="option">The option selected whose function should be called.</param>
        /// <returns>true if the option selected is not "quit".</returns>
        private bool HandleMenuOption(int option)
        {
            var menuActions = new Dictionary<int, Action>
            {
                { 1, AddHabit },
                { 2, RemoveHabit },
                { 3, ManageHabit },
                { 4, GetMaxStreaks },
                { 5, GetCurrentStreaks},
                { 6, Importer}
            };

            if (option == 9)
                return false;

            if (menuActions.TryGetValue(option, out var action))
                action();

            return true;
        }

        /// <summary>
        /// Wrapper to add habit in the TUI. Handles empty input.
        /// </summary>
        private void AddHabit()
        {
            _console.WriteLine("Please enter the name of your new habit: ");
            String name = GetStringInput();
            Habit newHabit = new Habit(0, name);
            _manager.AddHabit(newHabit);
        }

        /// <summary>
        /// Wrapper to remove habit in the TUI. Checks if habit exists using Linq.
        /// </summary>
        private void RemoveHabit()
        {
            _console.WriteLine("Please enter the name of the habit you want to remove: ");
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
                _console.WriteLine($"Could not find habit with name '{delete}'!");
            }
        }

        /// <summary>
        /// Interface to add or remove completion from habit.
        /// </summary>
        private void ManageHabit()
        {
            var habits = _manager.GetHabits();

            _console.WriteLine("Please choose a habit: ");
            PrintElementsWithIndex(habits);
            var habitId = GetNumberInput() - 1;

            var habit = new Habit(0, "Temp");

            try
            {
                habit = habits[habitId];
            }
            catch
            {
                _console.WriteLine("You must choose a valid ID!");
                _console.ReadKey();
                return;
            }
            _console.Clear();

            _console.WriteLine(habit.GetCompletionDates());
            _console.WriteLine("What do you want to do?");
            _console.WriteLine("1. Add Completion");
            _console.WriteLine("2. Remove Completion Date");
            _console.WriteLine("9. Cancel");

            _ = Int32.TryParse(_console.ReadLine(), out var index);

            switch (index)
            {
                case 1:
                    AddCompletion(habit);
                    break;
                case 2:
                    RemoveCompletion(habit);
                    break;
                default:
                    return;
            }
        }

        private void AddCompletion(Habit habit)
        {
            _console.Clear();
            _console.WriteLine("1. Add Completion Now");
            _console.WriteLine("2. Add Old Completion");

            _ = Int32.TryParse(_console.ReadLine(), out var index);
            switch (index)
            {
                case 1:
                    _manager.AddCompletion(habit.Id, DateTime.Now);
                    break;
                case 2:
                    AddOldCompletion(habit);
                    break;
                default:
                    break;
            }
        }

        private void AddOldCompletion(Habit habit)
        {
            _console.WriteLine(
            @"Please write the date in the following format: yyyy-MM-dd HH:mm:ss.
Example: 2025-03-14 16:39:00. You may omit the time.
Input Date:");

            var inputDate = _console.ReadLine();
            var parsedDate = DateTimeHelper.TryParseUserDate(inputDate);

            if (parsedDate.HasValue)  {
                _manager.AddCompletion(habit.Id, parsedDate.Value);
            } else {
                _console.WriteLine("Could not parse date! Press any key to continue.");
                _console.ReadKey();
            }
        }

        /// <summary>
        /// Removes a completion based on a list of completions outputted by the program.
        /// </summary>
        /// <param name="habit">The habit who has a completion to be deleted.</param>
        /// <exception cref="ArgumentException">If the input is not a valid index in the <see cref="Habit"/>'s completions.</exception>
        private void RemoveCompletion(Habit habit)
        {
            _console.WriteLine($"Habit: {habit.Name}. Please choose a completion you want to delete.");
            PrintElementsWithIndex(habit.Completions);
            var completionIndex = GetNumberInput();
            if (completionIndex > habit.Completions.Count || completionIndex < 0)
            {
                throw new ArgumentException("You must choose a valid index!");
            }
            completionIndex -= 1;
            _manager.RemoveCompletion(habit.Id, habit.Completions[completionIndex]);
            _console.WriteLine($"Completion {habit.Completions[completionIndex]} deleted.");
            _console.WriteLine("Press any key to continue.");
            _console.ReadKey();
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
                _console.WriteLine($"{i}: {element}");
            }
        }

        /// <summary>
        /// Get user input in the form of a number. If it is not a number, throw error.
        /// </summary>
        /// <returns>The number supplied by the user.</returns>
        /// <exception cref="ArgumentException">Throws ArgumentException error if the input is not an Int32 parseable number.</exception>
        private int GetNumberInput()
        {
            _console.Write("Number: ");
            bool success = Int32.TryParse(_console.ReadLine(), out var index);
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
            string? delete = _console.ReadLine();
            if (!string.IsNullOrWhiteSpace(delete))
            {
                return delete;
            }
            else
            {
                throw new ArgumentException("You must type a habit name.");
            }
        }

        private void GetMaxStreaks()
        {
            _console.WriteLine("=== Max Streaks ===");

            foreach (var h in _manager.GetHabits())
            {
                if (h.Completions.Any())
                    _console.WriteLine($"{h}: {h.GetLongestStreak()}");
            }

            _console.WriteLine("Press any key to continue...");
            _console.ReadKey();
        }

        private void GetCurrentStreaks()
        {
            _console.WriteLine("=== Current Streaks ===");

            foreach (var h in _manager.GetHabits())
            {
                if (h.Completions.Any())
                    _console.WriteLine($"{h}: {h.GetCurrentStreak()}");
            }

            _console.WriteLine("Press any key to continue...");
            _console.ReadKey();
        }

        private void Importer()
        {
            _console.WriteLine("=== Import Data ===");
            _console.WriteLine("Please be aware that currently there is only the option to import from Habitica. Further importing options will be available in the future.");
            _console.WriteLine("Importing your habits more than once will duplicate them.");

            _console.WriteLine("From the directory you opened HabitTracker in, please type the relative or absolute path to your Habitica habits user data. Leave blank if you want to go back.");
            _console.Write("File path (press enter to accept): ");
            var filePath = _console.ReadLine();

            if (filePath == null)
                return;

            ImportHabitica(filePath);
        }

        private void ImportHabitica(string filePath)
        {
            var importer = new Import(_manager);
            var habitica = new HabiticaImporter(importer);


            try
            {
                habitica.ImportData(filePath);
            }
            catch (Exception ex)
            {
                _console.WriteLine($"Import failed. Error: {ex.Message}");
            }
        }
    }
}
