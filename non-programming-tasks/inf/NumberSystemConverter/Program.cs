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
    static void Main()
    {
        TryCharToDecNumber('Z', out int num);
        Console.WriteLine(num);

        Console.WriteLine(ConvertFromDec(255, 2));
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
}
