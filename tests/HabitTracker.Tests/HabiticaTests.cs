using CsvHelper;

namespace HabitTracker.Tests;

[Collection("Sequential")]
public class HabiticaTests
{
    const string ValidCsv = "../../../TestData/Habitica1.csv";
    const string OneLineCsv = "../../../TestData/Habitica_OneLine.csv";
    const string Invalid1 = "../../../TestData/Invalid1.csv";
    const string Invalid2 = "../../../TestData/Invalid2.csv";
    const string Empty = "../../../TestData/Empty.csv";

    private static readonly Dictionary<string, int> completionsByHabit = new Dictionary<string, int> {
        {"Se 1 forelæsning", 5},
        {"Lav 1 lektion værd af opgaver", 2},
        {"Læs 1 kapitel i en bog", 4},
        {"Anki", 9},
        {"Allergivaccine", 7}
    };
    private static readonly Dictionary<string, List<DateTime>> habitDates = new Dictionary<string, List<DateTime>>()
        {
            { "Se 1 forelæsning", new List<DateTime>
                {
                    DateTimeHelper.Parse("2021-04-01 19:16:58"),
                    DateTimeHelper.Parse("2021-04-02 18:40:02"),
                    DateTimeHelper.Parse("2021-04-03 21:28:04"),
                    DateTimeHelper.Parse("2021-04-04 21:48:30"),
                    DateTimeHelper.Parse("2021-04-05 18:23:04")
                }
            },
            { "Lav 1 lektion værd af opgaver", new List<DateTime>
                {
                    DateTimeHelper.Parse("2021-04-03 13:07:15"),
                    DateTimeHelper.Parse("2021-04-05 18:23:09")
                }
            },
            { "Læs 1 kapitel i en bog", new List<DateTime>
                {
                    DateTimeHelper.Parse("2021-04-01 15:12:51"),
                    DateTimeHelper.Parse("2021-04-02 18:40:08"),
                    DateTimeHelper.Parse("2021-04-03 16:30:48"),
                    DateTimeHelper.Parse("2021-04-04 11:45:46")
                }
            },
            { "Anki", new List<DateTime>
                {
                    DateTimeHelper.Parse("2021-04-01 14:36:57"),
                    DateTimeHelper.Parse("2021-04-02 17:20:00"),
                    DateTimeHelper.Parse("2021-04-03 20:27:31"),
                    DateTimeHelper.Parse("2021-04-04 13:18:39"),
                    DateTimeHelper.Parse("2021-04-06 06:06:07"),
                    DateTimeHelper.Parse("2021-04-07 08:26:50"),
                    DateTimeHelper.Parse("2021-06-24 21:50:22"),
                    DateTimeHelper.Parse("2023-03-16 18:04:27"),
                    DateTimeHelper.Parse("2025-03-14 19:44:39")
                }
            },
            { "Allergivaccine", new List<DateTime>
                {
                    DateTimeHelper.Parse("2021-04-03 21:28:39"),
                    DateTimeHelper.Parse("2021-04-04 12:16:57"),
                    DateTimeHelper.Parse("2021-04-05 12:13:52"),
                    DateTimeHelper.Parse("2021-04-06 06:06:13"),
                    DateTimeHelper.Parse("2021-04-07 08:27:09"),
                    DateTimeHelper.Parse("2023-03-16 18:04:27"),
                    DateTimeHelper.Parse("2025-03-14 19:44:39")
                }
            }
        };

    private readonly IHabitRepository _repository;
    private readonly HabitManager _manager;
    private readonly IImportService _importer;
    private readonly HabiticaImporter _habiticaImporter;

    public HabiticaTests()
    {
        _repository = new HabitRepository("habits_test.db");
        _manager = new HabitManager(_repository);
        _importer = new Import(_manager);
        _habiticaImporter = new HabiticaImporter(_importer);

        _repository.InitializeDatabase();
    }

    [Fact]
    public void ImportHabits_ShouldImport_OneHabit()
    {
        var habits = _habiticaImporter.ImportHabits(OneLineCsv);
        var habit = habits.FirstOrDefault();


        Assert.NotNull(habit);
        Assert.Equal("Exercise", habit.Name.Trim());
        Assert.Equal(new DateTime(2021, 4, 1, 19, 16, 58),
                     habit.Completions[0]);
    }

    [Fact]
    public void ImportHabits_ShouldImport_MultipleHabits()
    {
        var habits = _habiticaImporter.ImportHabits(ValidCsv).ToList();

        Assert.NotNull(habits);
        Assert.Equal(5, habits.Count);

        var expectedHabits = new Dictionary<string, List<DateTime>>
    {
        { "Se 1 forelæsning", new List<DateTime>
            {
                new DateTime(2021, 4, 1, 19, 16, 58),
                new DateTime(2021, 4, 2, 18, 40, 02),
                new DateTime(2021, 4, 3, 21, 28, 04),
                new DateTime(2021, 4, 4, 21, 48, 30),
                new DateTime(2021, 4, 5, 18, 23, 04)
            }
        },
        { "Lav 1 lektion værd af opgaver", new List<DateTime>
            {
                new DateTime(2021, 4, 3, 13, 07, 15),
                new DateTime(2021, 4, 5, 18, 23, 09)
            }
        },
        { "Læs 1 kapitel i en bog", new List<DateTime>
            {
                new DateTime(2021, 4, 1, 15, 12, 51),
                new DateTime(2021, 4, 2, 18, 40, 08),
                new DateTime(2021, 4, 3, 16, 30, 48),
                new DateTime(2021, 4, 4, 11, 45, 46)
            }
        },
        { "Anki", new List<DateTime>
            {
                new DateTime(2021, 4, 1, 14, 36, 57),
                new DateTime(2021, 4, 2, 17, 20, 00),
                new DateTime(2021, 4, 3, 20, 27, 31),
                new DateTime(2021, 4, 4, 13, 18, 39),
                new DateTime(2021, 4, 6, 06, 06, 07),
                new DateTime(2021, 4, 7, 08, 26, 50),
                new DateTime(2021, 6, 24, 21, 50, 22),
                new DateTime(2023, 3, 16, 18, 04, 27),
                new DateTime(2025, 3, 14, 19, 44, 39)
            }
        },
        { "Allergivaccine", new List<DateTime>
            {
                new DateTime(2021, 4, 3, 21, 28, 39),
                new DateTime(2021, 4, 4, 12, 16, 57),
                new DateTime(2021, 4, 5, 12, 13, 52),
                new DateTime(2021, 4, 6, 06, 06, 13),
                new DateTime(2021, 4, 7, 08, 27, 09),
                new DateTime(2023, 3, 16, 18, 04, 27),
                new DateTime(2025, 3, 14, 19, 44, 39)
            }
        }
    };

        foreach (var expectedHabit in expectedHabits)
        {
            string habitName = expectedHabit.Key;
            List<DateTime> expectedCompletions = expectedHabit.Value;

            var importedHabit = habits.FirstOrDefault(h => h.Name.Trim() == habitName.Trim());
            Assert.NotNull(importedHabit);

            Assert.Equal(expectedCompletions.Count, importedHabit.Completions.Count);

            foreach (var expectedCompletion in expectedCompletions)
            {
                Assert.Contains(expectedCompletion, importedHabit.Completions);
            }
        }

    }

    [Fact]
    public void ImportHabits_ShouldHaveZeroHabits_OnEmptyFile()
    {
        Assert.Empty(_habiticaImporter.ImportHabits(Empty));
    }

    [Fact]
    public void ImportHabits_ShouldThrow_InvalidHeaders()
    {
        Assert.Throws<HeaderValidationException>(() => _habiticaImporter.ImportHabits(Invalid1));
    }

    [Fact]
    public void ImportData_ShouldImport_WithOneValidItem()
    {
        Assert.Empty(_manager.GetHabits());

        _habiticaImporter.ImportData(OneLineCsv);

        var importedHabits = _manager.GetHabits();
        Assert.Single(importedHabits);

        foreach (var habit in importedHabits)
        {
            Assert.Single(habit.Completions);
            _manager.RemoveHabit(habit.Id);
        }
    }

    [Fact]
    public void ImportData_ShouldImport_WithManyValidItems()
    {
        Assert.Empty(_manager.GetHabits());

        _habiticaImporter.ImportData(ValidCsv);


        var importedHabits = _manager.GetHabits();
        Assert.Equal(5, importedHabits.Count);

        foreach (var habit in importedHabits)
        {
            Assert.Equal(
                completionsByHabit[habit.Name.Trim()],
                habit.Completions.Count
            );

            Assert.Equal(
                habitDates[habit.Name.Trim()],
                habit.Completions
            );

            _manager.RemoveHabit(habit.Id);
        }
    }

    [Fact]
    public void ImportData_ShouldNotImport_InvalidItems()
    {
        Assert.Throws<HeaderValidationException>(() => _habiticaImporter.ImportData(Invalid1));
    }

    [Fact]
    public void ImportData_ShouldNotThrow_EmptyFile()
    {
        // If the task throws, the test fails automatically.
        _habiticaImporter.ImportData(Empty);
    }

}
