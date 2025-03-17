using CsvHelper;

namespace HabitTracker.Tests;

public class CsvHelperTests
{
    const string ValidCsv = "../../../TestData/Habitica1.csv";

    [Fact]
    public void GetCsvReader_ShouldReturnCsvReader_WhenFileExists()
    {
        var csvReader = CsvHelper.GetCsvReader(ValidCsv);

        Assert.NotNull(csvReader);
        Assert.IsType<CsvReader>(csvReader);
    }

    [Fact]
    public void GetCsvReader_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
    {
        var filePath = "non_existant.csv";

        Assert.Throws<FileNotFoundException>(() => CsvHelper.GetCsvReader(filePath));
    }

    [Fact]
    public void GetCsvReader_ShouldThrowArgumentException_WhenFilePathIsEmpty()
    {
        var filePath = "";

        Assert.Throws<ArgumentException>(() => CsvHelper.GetCsvReader(filePath));
    }
}
