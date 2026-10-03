namespace LogParse;
internal class Program
{
    public static (DateTime, string, string, string) GetLog(string buffer)
    {
        string[] firstLevelBuffer = buffer.Split("] "); // 2026-09-01 12:16:01.101 [Info][User
        string text = firstLevelBuffer[1]; // User logged in: RavenFox
        string[] secondLevelBuffer = firstLevelBuffer[0].Split("[");
        string level = secondLevelBuffer[1][..^1]; // Info
        string type = secondLevelBuffer[2]; // User

        DateTime datetime = DateTime.Parse(secondLevelBuffer[0][..^1]);

        return (datetime, level, type, text);
    }


    static void Main()
    {
        if (File.Exists("event_server.log"))
        {
            string[] logs = File.ReadAllLines("event_server.log");

            foreach (string log in logs)
            {
                Console.WriteLine(GetLog(log));
            }
        }
    }
}
