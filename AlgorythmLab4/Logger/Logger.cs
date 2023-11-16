namespace AlgorythmLab4.Logger;

public class Logger
{
    private List<IMessageHandler> _handlers = new();
    private static readonly Dictionary<int, Logger> _loggers = new();
    public int Id { get; private set; }
    public string Name { get; set; }
    public Level Level { get; set; }
    public int Delay { get; set; }

    public Logger(string name, Level level, IMessageHandler handler, int delay)
    {
        Initialize(name, level);
        _handlers.Add(handler);
        Delay = delay;
    }

    public Logger(string name, Level level, IEnumerable<IMessageHandler> handlers, int delay)
    {
        Initialize(name, level);
        _handlers.AddRange(handlers);
        Delay = delay;
    }

    public Logger(string name, int delay)
    {
        Initialize(name, Level.INFO);
        _handlers.Add(new ConsoleHandler());
        Delay = delay;
    }

    public Logger(string name, Level level, int delay)
    {
        Initialize(name, level);
        _handlers.Add(new ConsoleHandler());
        Delay = delay;
    }
    
    private void Initialize(string name, Level level)
    {
        Name = name;
        Level = level;
        _loggers.Add(IdentifierSetter.GetId(), this);
    }

    public void ClearHandlers() => _handlers = new List<IMessageHandler>();

    public void AddHandler(IMessageHandler handler) => _handlers.Add(handler);

    public static Logger GetLogger(int id, int delay) => _loggers.ContainsKey(id) ? _loggers[id] : new Logger("newLogger", delay);

    public static Logger GetLogger(int id, string name, Level level, int delay)
    {
        if (_loggers.ContainsKey(id))
        {
            _loggers[id].Level = level;
            return _loggers[id];
        }

        return new Logger(name, level, delay);
    }
    
    public static Logger GetLogger(int id, string name, Level level, IEnumerable<IMessageHandler> handlers, int delay)
    {
        if (_loggers.ContainsKey(id))
        {
            _loggers[id].Level = level;
            _loggers[id]._handlers.AddRange(handlers);
            return _loggers[id];
        }

        return new Logger(name, level, handlers, delay);
    }
    
    public void Log(Level level, string message) => LogMessage(level, message);
    
    public void Log(string message) => LogMessage(Level.INFO, message);
    
    private void LogMessage(Level level, string message)
    {
        if (level >= Level)
        {
            var logMessage = $"[{level}] {DateTime.Now:yyyy.MM.dd HH:mm:ss} {Name}  {message}";
            foreach (var handler in _handlers)
            {
                handler.Log(logMessage);
            }
        }
        
        Thread.Sleep(Delay);
    }
    
    public void Debug(string message) => Log(Level.DEBUG, message);
    
    public void Info(string message) => Log(Level.INFO, message);
    
    public void Warning(string message) => Log(Level.WARNING, message);
    
    public void Error(string message) => Log(Level.ERROR, message);
}