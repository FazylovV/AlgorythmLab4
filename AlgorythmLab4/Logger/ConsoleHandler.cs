namespace AlgorythmLab4.Logger;

public class ConsoleHandler : IMessageHandler
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}