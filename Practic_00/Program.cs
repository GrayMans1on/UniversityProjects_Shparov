namespace Practic_00
{
    internal class Program
    {
        public static int ConvertToNumber(string str)
        {
            int num = 0;
            foreach (char c in str)
            {
                num = num * 10 + (c - '0');
            }
            return num;
        }
        static void Main()
        {
            int num = ConvertToNumber(Console.ReadLine());
            // ...
        }
    }
}
