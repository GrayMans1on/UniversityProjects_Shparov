namespace ReportGenerator;
public class Program
{
    static void Main()
    {
        Console.WriteLine("Создаём отчёт по данным. Введите название события:");
        string title = Console.ReadLine();

        Console.WriteLine("\nВведите краткое описание события (будет обрезано до 200 символов):");
        string comments = Console.ReadLine();

        Console.WriteLine("\nВведите дату начала события (формат - день.месяц.год, напр.: 01.01.2001):");
        string dateStartBuffer = Console.ReadLine();

        Console.WriteLine("\nВведите дату окончания события (формат - день.месяц.год, напр.: 02.01.2001):");
        string dateEndBuffer = Console.ReadLine();

        Console.WriteLine("\nВведите количество участников:");
        string participantsCountBuffer = Console.ReadLine();

        Console.WriteLine("\nВведите имя победителя:");
        string winnerName = Console.ReadLine();

        Console.WriteLine("\nВведите оценку победителя (будет округлено до 2 знаков после запятой):");
        string winnerRatingBuffer = Console.ReadLine();

        Console.WriteLine("\nВведите максимально возможную оценку (будет округлено до 2 знаков после запятой):");
        string maxRatingBuffer = Console.ReadLine();

        Console.WriteLine("\nВведите приз победителя в рублях:");
        string prizeBuffer = Console.ReadLine();

        Console.WriteLine("\nВведите формат отчёта\nОсобые сиволы: TT - название события,\nCC - описание события,\nDD - дата начала,\ndd - дата окончания,\nUU - длительность события в днях,\nuu - длительность события в часах,\n" +
            "PP - количество участников,\nWW - имя победителя,\nRR - оценка победителя,\nMM - максимально возможная оценка,\npp - приз победителя,\n" +
            "EE - конец формата;\nОбратите внимание, что все форматы пишутся двумя английскими буквами без пробелов\n" +
            "Запишите формат (или \"Auto\" для автоматического формата):");
        string format = "";
        while (!format.Contains("EE"))
        {
            format += Console.ReadLine() + "\n";
        }
        format = format[..^3];

        Console.WriteLine("\n\nОтчёт:");
        Console.WriteLine(CreateReportFromString(format, title, comments, dateStartBuffer, dateEndBuffer, participantsCountBuffer, winnerName, winnerRatingBuffer, maxRatingBuffer, prizeBuffer));
    }

    public static string CreateReportFromString(string format, string title, string comments, string dateStartBuffer, string dateEndBuffer, string participantsCountBuffer, string winnerName,
        string winnerRatingBuffer, string maxRatingBuffer, string prizeBuffer)
    {
        List<string> exceptions = new List<string>();
        DateTime dateStart;
        if (!DateTime.TryParse(dateStartBuffer, out dateStart))
        {
            exceptions.Add("DD");
        }
        DateTime dateEnd;
        if (!DateTime.TryParse(dateEndBuffer, out dateEnd))
        {
            exceptions.Add("dd");
        }
        short participantsCount;
        if (!short.TryParse(participantsCountBuffer, out participantsCount))
        {
            exceptions.Add("PP");
        }
        float winnerRating;
        if (!float.TryParse(winnerRatingBuffer.Replace(".", ","), out winnerRating))
        {
            exceptions.Add("RR");
        }
        float maxRating;
        if (!float.TryParse(maxRatingBuffer.Replace(".", ","), out maxRating))
        {
            exceptions.Add("MM");
        }
        int prize;
        if (!int.TryParse(prizeBuffer, out prize))
        {
            exceptions.Add("pp");
        }

        if (string.IsNullOrEmpty(format) || format.ToLower() == "auto")
        {
            return CreateReport(title, comments, dateStart, dateEnd, participantsCount, winnerName, winnerRating, maxRating, prize, exceptions.ToArray());
        }
        return CreateReport(title, comments, dateStart, dateEnd, participantsCount, winnerName, winnerRating, maxRating, prize, exceptions.ToArray(), format);
    }
    private static string CreateReport(string title, string comments, DateTime dateStart, DateTime dateEnd, short participantsCount, string winnerName, float winnerRating, float maxRating, int prize, string[] exceptions,
        string format = "{ TT }\n" +
                        "\nДоп. комментарии: CC;\n" +
                        "\nДата начала: DD;\n" +
                        "\nДата окончания: dd;\n" +
                        "\nДлительность: uu часов;\n" +
                        "\nКоличество участников: PP;\n" +
                        "\nПобедитель: WW;\n" +
                        "\nОценка победителя: RR/MM;\n" +
                        "\nПобедный приз: pp руб.")
    {
        string result = "";
        if (string.IsNullOrEmpty(title))
        {
            title = "Событие без названия";
        }
        if (string.IsNullOrEmpty(comments))
        {
            comments = "нет комментария";
        }
        if (comments.Length > 200)
        {
            comments = comments[..200];
        }
        for (int i = 0, j = 1; j < format.Length; i++, j++)
        {
            switch (format[i..(j + 1)])
            {
                case "TT": // title
                    result += title;
                    i++;
                    j++;
                    break;
                case "CC": // comments
                    result += comments;
                    i++;
                    j++;
                    break;
                case "DD": // date start
                    if (!exceptions.Contains("DD"))
                    {
                        result += dateStart.ToString("dd.MM.yy");
                    }
                    else
                    {
                        result += "#";
                    }
                    i++;
                    j++;
                    break;
                case "dd": // date end
                    if (!exceptions.Contains("dd"))
                    {
                        result += dateEnd.ToString("dd.MM.yy");
                    }
                    else
                    {
                        result += "#";
                    }
                    i++;
                    j++;
                    break;
                case "UU": // duration (days)
                    if (!exceptions.Contains("DD") && !exceptions.Contains("dd"))
                    {
                        TimeSpan durationDays = dateEnd - dateStart;
                        result += durationDays.Days;
                    }
                    else
                    {
                        result += "#";
                    }
                    i++;
                    j++;
                    break;
                case "uu": // duration (hours)
                    if (!exceptions.Contains("DD") && !exceptions.Contains("dd"))
                    {
                        TimeSpan durationHours = dateEnd - dateStart;
                        result += durationHours.Days * 24 + durationHours.Hours;
                    }
                    else
                    {
                        result += "#";
                    }
                    i++;
                    j++;
                    break;
                case "PP": // participants count
                    if (!exceptions.Contains("PP"))
                    {
                        result += participantsCount;
                    }
                    else
                    {
                        result += "#";
                    }
                    i++;
                    j++;
                    break;
                case "WW": // winner name
                    result += winnerName;
                    i++;
                    j++;
                    break;
                case "RR": // winner rating
                    if (!exceptions.Contains("RR"))
                    {
                        result += Math.Round(winnerRating * 100) / 100;
                    }
                    else
                    {
                        result += "#";
                    }
                    i++;
                    j++;
                    break;
                case "MM": // max rating
                    if (!exceptions.Contains("MM"))
                    {
                        result += Math.Round(maxRating * 100) / 100;
                    }
                    else
                    {
                        result += "#";
                    }
                    i++;
                    j++;
                    break;
                case "pp": // prize
                    if (!exceptions.Contains("pp"))
                    {
                        result += prize;
                    }
                    else
                    {
                        result += "#";
                    }
                    i++;
                    j++;
                    break;
                default: // not a format string
                    result += format[i];
                    break;
            }
        }
        result += format[^1];

        return result;
    }
}
