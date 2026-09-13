using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Globalization;

namespace Lab_01
{
    internal class Program
    {
        public static void SaveData(PlayerData[] data)
        {
            string jsonString = JsonSerializer.Serialize(data);
            File.WriteAllText(_dataPath, jsonString);
        }
        public static PlayerData[] GetData()
        {
            if (!File.Exists(_dataPath))
            {
                File.WriteAllText(_dataPath, "[]");
                Console.WriteLine("Файла не существует, создаём\n");
            }

            string jsonString = File.ReadAllText(_dataPath);

            try
            {
                PlayerData[] data = JsonSerializer.Deserialize<PlayerData[]>(jsonString); // хранение данных как json массив
                if (data != null)
                {
                    return data;
                }
                return [];
            }
            catch // исключение на случай повреждения файла извне
            {
                return [];
            }
        }
        public static string DataSummary(PlayerData[] data)
        {
            int count = data.Length;
            if (count == 0)
            {
                return "0 записей";
            }
            PlayerData lastSeenPlayer = data[0];

            float levelSum = 0;

            foreach (PlayerData dataItem in data) // вычисление последнего активного игрока
            {
                levelSum += dataItem.Level;
                if (dataItem.LastSeen > lastSeenPlayer.LastSeen)
                {
                    lastSeenPlayer = dataItem;
                }
            }

            return $"Сводка:\nКоличество зарегестрированных пользователей: {count}\nСредний уровень учётной записи: {(float)levelSum / (float)count}\n" +
                $"\nПоследний активный пользователь:\nID: {lastSeenPlayer.UserId}\nИмя: {lastSeenPlayer.Name}\nКоординаты: {lastSeenPlayer.XCoord}; {lastSeenPlayer.YCoord}" +
                $"\nБыл активен: {(DateTime.Now - lastSeenPlayer.LastSeen).Days} дней назад";
        }

        public static string _dataPath = "data.json";
        public static PlayerData[] _playerDatas;
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("Введите:\n0 - посмотреть данные\n1 - сделать новую запись\n2 - сменить файл\n3 (или другой символ) - закрыть");
                string typeBuffer = Console.ReadLine();
                switch (typeBuffer)
                {
                    case "0":
                        _playerDatas = GetData();
                        if (_playerDatas.Length == 0)
                        {
                            Console.WriteLine($"\nДанные в {_dataPath} прочтены. Записей нет или они повреждены!");
                            break;
                        }

                        Console.WriteLine($"\nДанные в {_dataPath} успешно прочтены! Введите:\n0 для общей сводки\nНомер страницы от 1 до {_playerDatas.Length}, чтобы открыть конкретную запись\nЛюбую другой символ, чтобы выйти назад");
                        string dataReadTypeBuffer = Console.ReadLine();
                        if (dataReadTypeBuffer == "0")
                        {
                            Console.WriteLine(DataSummary(_playerDatas));
                            break;
                        }
                        if (int.TryParse(dataReadTypeBuffer, out int num))
                        {
                            if (num <= 0 || num > _playerDatas.Length)
                            {
                                Console.WriteLine();
                                break;
                            }
                            PlayerData data = _playerDatas[num - 1];
                            Console.WriteLine($"\nДанные по {num}-ой записи:\nИмя: {data.Name}\nID: {data.UserId}\nЗаходил последний раз: {(DateTime.Now - data.LastSeen).Days} дней назад" +
                                $"\nКоординаты: {data.XCoord}; {data.YCoord}\nУровень учётной записи: {data.Level}");
                        }
                        Console.WriteLine();
                        break;
                    case "1":
                        Console.WriteLine("\nСоздаём новую запись, введите имя игрока:");
                        string name = Console.ReadLine();
                        if (name == "")
                        {
                            Console.WriteLine("Недействительное имя\n");
                            break;
                        }

                        Console.WriteLine("Введите уровень учётной записи (натуральное число):");
                        string levelBuffer = Console.ReadLine();
                        short level = 1;
                        if (short.TryParse(levelBuffer, out level))
                        {
                            if (level <= 0)
                            {
                                Console.WriteLine("Недействительное число\n");
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Недействительное число\n");
                            break;
                        }

                        Console.WriteLine("Введите координаты X и Y через пробел:");
                        string coordsBuffer = Console.ReadLine().Replace(".", ",");
                        if (!coordsBuffer.Contains(" "))
                        {
                            Console.WriteLine("Недействительная запись\n");
                            break;
                        }
                        string[] coordsSplitBuffer = coordsBuffer.Split(" ");
                        if (coordsBuffer.Length > 2)
                        {
                            Console.WriteLine("Недействительная координаты\n");
                            break;
                        }
                        string xBuffer = coordsBuffer.Split(" ")[0];
                        string yBuffer = coordsBuffer.Split(" ")[1];
                        double xCoord = 0F;
                        double yCoord = 0F;
                        if (!double.TryParse(xBuffer, out xCoord))
                        {
                            Console.WriteLine("Недействительная координата X\n");
                            break;
                        }
                        if (!double.TryParse(yBuffer, out yCoord))
                        {
                            Console.WriteLine("Недействительная координата Y\n");
                            break;
                        }

                        Console.WriteLine("Введите дату последнего захода в игру в формате День.Месяц.Год, например 04.05.2026:");
                        string dateBuffer = Console.ReadLine();
                        DateTime date;
                        if (!DateTime.TryParseExact(dateBuffer, "dd.MM.yyyy", null, DateTimeStyles.None, out date))
                        {
                            Console.WriteLine("Недействительная дата\n");
                            break;
                        }
                        if ((DateTime.Now - date).Days < 0)
                        {
                            Console.WriteLine("Недействительная дата\n");
                            break;
                        }


                        PlayerData newData = new PlayerData(new Random().Next(), name, xCoord, yCoord, level, date);


                        _playerDatas = GetData();
                        List<PlayerData> cachedData = _playerDatas.ToList();
                        cachedData.Add(newData);
                        _playerDatas = cachedData.ToArray();
                        SaveData(_playerDatas);
                        Console.WriteLine("Данные успешно добавлены!\n");

                        break;
                    case "2":
                        Console.WriteLine("\nВведите название файла с расширением .json, например data.json:");
                        string fileName = Console.ReadLine();
                        if (fileName.Length > 5 && fileName[^5..] == ".json" && fileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0)
                        {
                            _dataPath = fileName;
                            Console.WriteLine($"Смена файла на {_dataPath}\n");
                        }
                        else
                        {
                            Console.WriteLine("Недопустимое имя файла\n");
                        }
                        break;
                    default:
                        Console.WriteLine("Приложение закрыто");
                        return;
                }
            }
        }
    }

    class PlayerData // класс для данных конкретного игрока
    {
        private int _userId;
        public int UserId => _userId;


        private string _name = "Player";
        public string Name => _name;


        private double _xCoord;
        public double XCoord => _xCoord;
        private double _yCoord;
        public double YCoord => _yCoord;


        private short _level;
        public short Level => _level;


        private DateTime _lastSeen;
        public DateTime LastSeen => _lastSeen;



        public PlayerData(int userId, string name, double xCoord, double yCoord, short level, DateTime lastSeen)
        {
            _userId = userId;
            _name = name;
            _lastSeen = lastSeen;
            _xCoord = xCoord;
            _yCoord = yCoord;
            _level = level;
        }
    }
}
