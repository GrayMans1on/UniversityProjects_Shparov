namespace ServerConfig;

public class Program
{
    static void Main()
    {
        Console.WriteLine("Запускаем сервер. Введите параметры:\nОжидаемое количество игроков:");
        int players = int.Parse(Console.ReadLine());

        Console.WriteLine("Доступное количество оперативной памяти (ГБ): ");
        int ram = int.Parse(Console.ReadLine());

        Console.WriteLine("Включать античит? (Y - да, любой другой символ - нет): ");
        bool anticheat = Console.ReadLine() == "Y";

        Console.WriteLine("Использовать публичный сервер с паролем? (Y - да, любой другой символ - нет): ");
        bool password = Console.ReadLine() == "Y";

        Console.WriteLine(CheckConfiguration(players, ram, anticheat, password));
    }
    private static int ExpectedMemory(int playersCount)
    {
        return 2 + (int)(playersCount / 10F);
    }
    public static string CheckConfiguration(int playersCount, int ramAmount, bool useAnticheat, bool passwordAccess)
    {
        if (playersCount <= 0)
        {
            return "Запуск невозможен: количество игроков должно быть больше нуля.";
        }
        if (ramAmount <= 2)
        {
            return "Запуск невозможен: серверу недостаточно оперативной памяти.";
        }
        if (ramAmount < ExpectedMemory(playersCount))
        {
            return "Запуск возможен с предупреждением: для такого количества игроков рекомендуется больше оперативной памяти.";
        }
        if (passwordAccess)
        {
            return "Запуск возможен с предупреждением: публичный сервер защищён паролем.";
        }
        if (!useAnticheat)
        {
            return "Сервер готов к запуску. Анти-чит будет выключен.";
        }
        return "Сервер готов к запуску.";
    }
}
