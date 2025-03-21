namespace HabitTracker.Tests;

public class SystemConsoleTests
{
    private readonly SystemConsole _systemConsole;

    public SystemConsoleTests()
    {
        _systemConsole = new SystemConsole();
    }

    [Fact]
    public void ReadLine_Works_WithoutInput()
    {
        var input = new StringReader("");
        Console.SetIn(input);
        Assert.Equal("", _systemConsole.ReadLine());
    }

    [Fact]
    public void ReadKey_Works_WithBlankInput()
    {
        var input = new StringReader("");
        Console.SetIn(input);
        Assert.Equal("", _systemConsole.ReadKey());
    }

    [Fact]
    public void ReadKey_ReturnsCorrectKey_WithInput()
    {
        Console.SetIn(new StringReader("a"));
        Assert.Equal("a", _systemConsole.ReadKey());
    }
}