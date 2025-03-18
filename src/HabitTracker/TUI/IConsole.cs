// <summary>
// The purpose of this interface is to allow for testing of the TUI. By creating
// an interface, we can mock the usual Console methods.
// </summary>
public interface IConsole
{
    void Clear();
    void WriteLine(string message);
    string ReadLine();
    void Write(string message);
    string ReadKey();
}
