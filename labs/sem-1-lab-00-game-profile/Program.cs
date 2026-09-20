namespace Lab_00
{
    class GameProfileCreator
    {
        
        static void Main()
        {
            string[] profilePictures = {
                "   ",
                "0_0",
                "._.",
                "{}.{}",
                "'_'",
                "O_O",
                "$_$",
                "=_=",
                ">_<",
                "@_@"
            };
            string name = "Player";
            string description = "";
            int age = -1;
            string gender = "";
            int profilePictureID = 0;

            Console.WriteLine("Введите ваше имя:");
            name = Console.ReadLine();

            Console.WriteLine("\nВведите ваш возраст: ");
            string ageBuffer = Console.ReadLine();
            int ageMistakeCounter = 0;
            while (!int.TryParse(ageBuffer, out age) || age <= 0)
            {
                Console.WriteLine($"Возраст должен быть целым числом больше нуля. Осталось попыток: {5 - ageMistakeCounter}");
                ageBuffer = Console.ReadLine();
                ageMistakeCounter++;
                if (ageMistakeCounter >= 5)
                {
                    Console.WriteLine("Количество возможных попыток исчерпано!");
                    return;
                }
            }

            Console.WriteLine("\nВведите краткое описание профиля (не более 200 символов): ");
            string descriptionBuffer = Console.ReadLine();
            if (descriptionBuffer.Length > 200)
            {
                description = descriptionBuffer[..200];
                Console.WriteLine("Описание сокращено до 200 символов!");
            }
            else
            {
                description = descriptionBuffer;
            }
            Console.WriteLine("\nВыберите ваш пол: М - Мужской, Ж - Женский, любой другой символ - Не указано");
            gender = Console.ReadLine().ToUpper();
            if (gender != "М" && gender != "Ж")
            {
                gender = "";
            }
            Console.Write("Выберите иконку вашего персонажа (введите цифру от 0 до 9):\n" +
                "Без иконки | 0_0 | ._. | {}.{} | '_' | O_O | $_$ | =_= | >_< | @_@\n" +
                "    0      |  1  |  2  |   3   |  4  |  5  |  6  |  7  |  8  |  9\n");
            string pictureBuffer = Console.ReadLine();
            int pictureMistakeCounter = 0;
            while (!int.TryParse(pictureBuffer, out profilePictureID) || profilePictureID < 0 || profilePictureID > 9)
            {
                Console.WriteLine($"Выберите цифру от 0 до 9. Осталось попыток: {5 - pictureMistakeCounter}");
                pictureBuffer = Console.ReadLine();
                pictureMistakeCounter++;
                if (pictureMistakeCounter >= 5)
                {
                    Console.WriteLine("Количество возможных попыток исчерпано!");
                    return;
                }
            }
            Console.Write("\nИгровой профиль успешно создан:\n\n");
            string addInfo = "";
            if (gender != "")
            {
                addInfo = $"{gender}, {age}";
            }
            else
            {
                addInfo = age.ToString();
            }
            Console.Write($"{profilePictures[profilePictureID]} | {name} ({addInfo})\n" +
                $"Описание: {description}\n");
        }
    }
}
