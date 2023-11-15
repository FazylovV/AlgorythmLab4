using System.Text;
namespace AlgorythmLab4.Logger;

public class FileHandler : IMessageHandler
{
    private string _fileName;

    public FileHandler()
    {
        _fileName = "log";
    }

    public FileHandler(string fileName)
    {
        _fileName = fileName;
    }
    
    public void SetFileName(string fileName)
    {
        _fileName = fileName;
    }
    
    public void Log(string message)
    {
        using var writer = new StreamWriter($"{_fileName}.txt", append: true, Encoding.UTF8);
        writer.AutoFlush = true;
        writer.WriteLine(message);
    }
}
