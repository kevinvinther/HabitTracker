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
    public string ReadLine() => _inputQueue.Count > 0 ? _inputQueue.Dequeue() : string.Empty;
    public void Write(string message) => Output.Add($"[Write] {message}");
    public string ReadKey() => _inputQueue.Count > 0 ? _inputQueue.Dequeue() : string.Empty;

}
