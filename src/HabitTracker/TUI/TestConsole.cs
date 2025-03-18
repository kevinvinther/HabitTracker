public class TestConsole : IConsole
{
    private readonly Queue<string> _inputQueue;
    public List<string> Output { get; } = new List<string>();

    public TestConsole(Queue<string> inputQueue)
    {
        _inputQueue = inputQueue;
    }

    public void Clear() => Output.Add("[Clear]");
    public void WriteLine(string message) => Output.Add($"[WriteLine] {message}");

    public string ReadLine()
    {
        if (_inputQueue.Count == 0) throw new InvalidOperationException("No input available.");

        return _inputQueue.Dequeue();
    }

    public void Write(string message) => Output.Add($"[Write] {message}");

    public string ReadKey()
    {
        if (_inputQueue.Count == 0) throw new InvalidOperationException("No key press available.");

        return _inputQueue.Dequeue();
    }
}