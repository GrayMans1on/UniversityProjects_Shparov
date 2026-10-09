namespace NumberSystemConverter;

internal class Program
{
    static int CharToDigit(char symbol)
    {
        if (symbol >= '0' && symbol <= '9')
        {
            return symbol - '0';
        }

        return -1;
    }

    static char DigitToChar(int digit)
    {
        if (digit > 9 || digit < 0)
        {
            return '\0';
        }

        return (char)('0' + digit);
    }

    static int SubscriptCharToDigit(char symbol)
    {
        if (symbol >= '₀' && symbol <= '₉')
        {
            return symbol - '₀';
        }

        return -1;
    }

    static char DigitToSubscriptChar(int digit)
    {
        if (digit > 9 || digit < 0)
        {
            return '\0';
        }

        return (char)('₀' + digit);
    }
    static int StringToNumber(string symbols)
    {
        int number = 0;

        for (int i = 0; i < symbols.Length; i++)
        {
            int digit = CharToDigit(symbols[i]);

            if (digit == -1)
            {
                return -1;
            }

            number = number * 10 + digit;
        }

        return number;
    }

    static string NumberToString(int number)
    {
        if (number < 0)
        {
            return "";
        }

        if (number == 0)
        {
            return DigitToChar(0).ToString();
        }

        string result = "";

        while (number > 0)
        {
            int digit = number % 10;
            result = DigitToChar(digit) + result;
            number /= 10;
        }

        return result;
    }

    static int SubscriptStringToNumber(string symbols)
    {
        int number = 0;

        for (int i = 0; i < symbols.Length; i++)
        {
            int digit = SubscriptCharToDigit(symbols[i]);

            if (digit == -1)
            {
                return -1;
            }

            number = number * 10 + digit;
        }

        return number;
    }

    static string NumberToSubscriptString(int number)
    {
        if (number < 0)
        {
            return "";
        }

        if (number == 0)
        {
            return DigitToSubscriptChar(0).ToString();
        }

        string result = "";

        while (number > 0)
        {
            int digit = number % 10;
            result = DigitToSubscriptChar(digit) + result;
            number /= 10;
        }

        return result;
    }
    private static bool TryCharToDecNumber(char c, out int num)
    {
        num = 0;
        if (c >= '0' && c <= '9')
        {
            num = c - '0';
            return true;
        }
        if (c >= 'A' && c <= 'Z')
        {
            num = c + 10 - 'A';
            return true;
        }
        return false;
    }
    public static int ConvertToDec(string num, int systemBase)
    {
        int result = 0;
        foreach (char c in num)
        {
            if (TryCharToDecNumber(c, out int value))
            {
                if (value >= systemBase) { return -1; }
                result = result * systemBase + value;
            }
            else
            {
                return -1;
            }
        }
        return result;
    }
    private static bool TryDecNumberToChar(int number, out char c)
    {
        c = '\0';
        if (number >= 0 && number <= 9)
        {
            c = (char)('0' + number);
            return true;
        }
        if (number >= 10 && number <= 35)
        {
            c = (char)('A' + number - 10);
            return true;
        }

        return false;
    }
    public static string ConvertFromDec(int num, int systemBaseTo)
    {
        string result = "";
        while (num > 0)
        {
            int remain = num % systemBaseTo;
            if (TryDecNumberToChar(remain, out char c))
            {
                result = c + result;
                num /= systemBaseTo;
            }
            else
            {
                return "";
            }
        }
        return result;
    }
    public static int ExplainConvertToDec(string num, int systemBase)
    {
        int result = 0;

        Console.WriteLine();
        Console.WriteLine($"Перевод {num} из системы с основанием {systemBase} в десятичную:");
        Console.WriteLine();

        for (int i = 0; i < num.Length; i++)
        {
            if (!TryCharToDecNumber(num[i], out int value))
            {
                Console.WriteLine($"Ошибка: недопустимый символ '{num[i]}'.");
                return -1;
            }

            if (value >= systemBase)
            {
                Console.WriteLine($"Ошибка: цифра '{num[i]}' не существует в этой системе.");
                return -1;
            }

            int power = num.Length - i - 1;
            int part = value * (int)Math.Pow(systemBase, power);

            Console.WriteLine(
                $"{num[i]} = {value}, разряд: {power}, " +
                $"{value} * {systemBase}^{power} = {part}");

            result += part;
        }

        Console.WriteLine();
        Console.WriteLine($"Сумма = {result}");

        return result;
    }
    public static string ExplainConvertFromDec(int num, int systemBaseTo)
    {
        if (num == 0)
        {
            Console.WriteLine("0 в любой системе счисления остаётся 0.");
            return "0";
        }

        string result = "";
        int current = num;

        Console.WriteLine();
        Console.WriteLine($"Перевод {num} из десятичной системы в систему с основанием {systemBaseTo}:");
        Console.WriteLine();

        int step = 1;

        while (current > 0)
        {
            int remain = current % systemBaseTo;
            int quotient = current / systemBaseTo;

            if (!TryDecNumberToChar(remain, out char c))
            {
                Console.WriteLine("Ошибка при переводе остатка.");
                return "";
            }

            Console.WriteLine(
                $"Шаг {step}: {current} / {systemBaseTo} = {quotient}, остаток {remain} ({c})");

            result = c + result;
            current = quotient;
            step++;
        }

        Console.WriteLine();
        Console.WriteLine("Читаем остатки снизу вверх:");
        Console.WriteLine(result);

        Console.WriteLine();
        Console.WriteLine($"Результат: {num} = {result}");

        return result;
    }
    public static string ExplainConvert(string num, int systemBaseFrom, int systemBaseTo)
    {
        Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
        Console.WriteLine("ПЕРЕВОД СИСТЕМ СЧИСЛЕНИЯ");
        Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
        if (systemBaseFrom == 10)
        {
            if (int.TryParse(num, out int newNum))
            {
                return ExplainConvertFromDec(newNum, systemBaseTo);
            }
            else
            {
                return "";
            }
        }
        else if (systemBaseTo == 10)
        {
            return ExplainConvertToDec(num, systemBaseFrom).ToString();
        }

        Console.WriteLine();
        Console.WriteLine("Этап 1. Перевод в десятичную систему.");

        int decimalNumber = ExplainConvertToDec(num, systemBaseFrom);

        if (decimalNumber == -1)
        {
            return "";
        }

        Console.WriteLine();
        Console.WriteLine("Этап 2. Перевод из десятичной системы.");

        string result = ExplainConvertFromDec(decimalNumber, systemBaseTo);

        Console.WriteLine();
        Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
        Console.WriteLine($"Ответ: {num}({systemBaseFrom}) = {result}({systemBaseTo})");
        Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");

        return result;
    }
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=\n" +
                          "   КАЛЬКУЛЯТОР СИСТЕМ СЧИСЛЕНИЯ\n" +
                          "=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=\n\n" +
                          "Поддерживаются системы счисления от 2 до 36\n" +
                          "Для цифр больше 9 используются буквы A-Z\n");

        while (true)
        {
            Console.Write("Введите число: ");
            string num = Console.ReadLine().ToUpper();

            if (num == "")
            {
                Console.WriteLine("Ошибка: число не может быть пустым\n");
                continue;
            }

            Console.Write("Введите основание исходной системы: ");

            if (!int.TryParse(Console.ReadLine(), out int systemBaseFrom))
            {
                Console.WriteLine("Ошибка: основание должно быть целым числом\n");
                continue;
            }

            if (systemBaseFrom < 2 || systemBaseFrom > 36)
            {
                Console.WriteLine("Ошибка: основание должно быть от 2 до 36!\n");
                continue;
            }

            Console.Write("Введите основание новой системы: ");

            if (!int.TryParse(Console.ReadLine(), out int systemBaseTo))
            {
                Console.WriteLine("Ошибка: основание должно быть целым числом\n");
                continue;
            }

            if (systemBaseTo < 2 || systemBaseTo > 36)
            {
                Console.WriteLine("Ошибка: основание должно быть от 2 до 36!\n");
                continue;
            }

            Console.WriteLine();

            string result = ExplainConvert(num, systemBaseFrom, systemBaseTo);

            if (result == "")
            {
                Console.WriteLine("\nПеревод выполнить не удалось!");
            }

            Console.WriteLine("\n=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Console.Write("Выполнить ещё один перевод? (Y/N): ");

            string answer = Console.ReadLine().ToUpper();

            if (answer != "Y")
            {
                break;
            }

            Console.WriteLine("\n\n");
        }

        Console.WriteLine();
        Console.WriteLine("Работа программы завершена.");
    }
}
