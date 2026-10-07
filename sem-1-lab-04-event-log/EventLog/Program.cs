namespace EventLog;
public enum Level
{
    Info,
    Warning,
    Error,
    Fatal,
}
public enum Category
{
    Server,
    User,
    Clan,
    Event,
    NPC,
    Combat,
    Loot,
    Trap,
    Reward,
    Statistics,
    Database,
}
public class Program
{
    static void Main()
    {
        LogEntry[] logs = ParseLog(File.ReadAllLines("..\\..\\..\\event_server.log"));

        Console.WriteLine(GetServerStatus(logs));
    }

    public static LogEntry GetLog(string line)
    {
        // e.g.: line = 2026-09-01 12:16:01.101 [Info][User] User logged in: RavenFox

        string[] firstLevelBuffer = line.Split("] "); // 2026-09-01 12:16:01.101 [Info][User
        string text = firstLevelBuffer[1]; // User logged in: RavenFox

        string[] secondLevelBuffer = firstLevelBuffer[0].Split("[");
        string levelBuffer = secondLevelBuffer[1][..^1]; // Info
        string categoryBuffer = secondLevelBuffer[2]; // User

        Level level = Enum.TryParse<Level>(levelBuffer, out Level cachedLevel)
            ? cachedLevel
            : Level.Fatal; // если логи битые, то пусть выводит фатальную ошибку
        Category category = Enum.TryParse<Category>(categoryBuffer, out Category cachedCategory)
            ? cachedCategory
            : Category.Server;

        DateTime datetime = DateTime.Parse(secondLevelBuffer[0][..^1]);


        return new LogEntry(datetime, level, category, text);
    }
    public static LogEntry[] ParseLog(string[] lines)
    {
        LogEntry[] logs = new LogEntry[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            logs[i] = GetLog(lines[i]);
        }
        return logs;
    }
    public static string LogToString(LogEntry log)
    {
        return $"{log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff")} [{log.Level}][{log.Category}] {log.Message}";
    }
    public static string[] LogsToText(LogEntry[] entries)
    {
        string[] result = new string[entries.Length];
        for (int i = 0; i < entries.Length; i++)
        {
            result[i] = LogToString(entries[i]);
        }
        return result;
    }

    public static LogEntry[] FilterByDate(LogEntry[] entries, DateTime date)
    {
        List<LogEntry> logs = new List<LogEntry>();
        foreach (LogEntry log in entries)
        {
            if (log.Timestamp.Day == date.Day)
            {
                logs.Add(log);
            }
        }
        return logs.ToArray();
    }
    public static LogEntry[] FilterByLevel(LogEntry[] entries, Level level)
    {
        List<LogEntry> logs = new List<LogEntry>();
        foreach (LogEntry log in entries)
        {
            if (log.Level == level)
            {
                logs.Add(log);
            }
        }
        return logs.ToArray();
    }
    public static LogEntry[] FilterByCategory(LogEntry[] entries, Category category)
    {
        List<LogEntry> logs = new List<LogEntry>();
        foreach (LogEntry log in entries)
        {
            if (log.Category == category)
            {
                logs.Add(log);
            }
        }
        return logs.ToArray();
    }
    public static LogEntry[] Search(LogEntry[] entries, string text)
    {
        List<LogEntry> logs = new List<LogEntry>();
        foreach (LogEntry log in entries)
        {
            if (log.Message.ToLower().Contains(text.ToLower()))
            {
                logs.Add(log);
            }
        }
        return logs.ToArray();
    }
    public static int CountByLevel(LogEntry[] entries, Level level)
    {
        int count = 0;
        foreach (LogEntry log in entries)
        {
            if (log.Level == level)
            {
                count++;
            }
        }
        return count;
    }
    public static string GetServerStatus(LogEntry[] entries)
    {
        foreach (LogEntry log in entries)
        {
            if (log.Level == Level.Fatal && log.Category == Category.Server)
            {
                return "КРИТИЧЕСКАЯ ОШИБКА: сервер остановлен";
            }
        }
        foreach (LogEntry log in entries)
        {
            if (log.Level == Level.Error)
            {
                return "Есть ошибки: требуется проверка";
            }
        }
        return "Сервер работает штатно";
    }
    public static void ExportFiltered(string path, LogEntry[] entries)
    {
        string[] lines = LogsToText(entries);
        File.WriteAllLines(path, lines);
    }
}


// struct выбран, потому что нет необохдимости обращаться к LogEntry как к ссылке,
// программа не предполагает изменение внутри конкретного лога,
// лог предполагается как читаемая и создаваемая единая структура
public struct LogEntry
{
    public DateTime Timestamp;
    public Level Level;
    public Category Category;
    public string Message;

    public LogEntry(DateTime dateTime, Level level, Category category, string message)
    {
        Timestamp = dateTime;
        Level = level;
        Category = category;
        Message = message;
    }
}
