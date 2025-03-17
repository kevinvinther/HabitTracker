public class SystemConsole : IConsole
{
    public void Clear() => Console.Clear();
    public void WriteLine(string message) => Console.WriteLine(message);
    public string ReadLine() => Console.ReadLine() ?? "";
    public void Write(string message) => Console.Write(message);
    public string ReadKey() => Console.ReadKey().ToString() ?? "";
}
