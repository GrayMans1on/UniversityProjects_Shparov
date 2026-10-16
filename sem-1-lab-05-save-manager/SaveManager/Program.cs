using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace SaveManager;
public class Program
{
    static void Main()
    {
        string _indexPath = "..\\..\\..\\index.json";
        string _savesDirectory = "..\\..\\..\\saves";
        string _backupsDirectory = "..\\..\\..\\backups";

        Directory.CreateDirectory(_savesDirectory);
        Directory.CreateDirectory(_backupsDirectory);

        SaveEntry[] entries = LoadIndex(_indexPath);

        while (true)
        {
            Console.WriteLine("\nМенеджер сохранений");
            Console.WriteLine("1 - Показать список сохранений");
            Console.WriteLine("2 - Посмотреть данные сохранения");
            Console.WriteLine("3 - Изменить данные персонажа");
            Console.WriteLine("4 - Создать резервную копию");
            Console.WriteLine("5 - Восстановить резервную копию");
            Console.WriteLine("6 - Удалить сохранение");
            Console.WriteLine("7 - Выйти (или любой другой символ)");
            Console.Write("Введите действие: ");

            string command = Console.ReadLine();

            switch (command)
            {
                case "1":
                    {
                        Console.WriteLine("\nСписок сохранений:");

                        foreach (SaveEntry entry in entries)
                        {
                            Console.WriteLine(
                                entry.Name + " | " +
                                entry.File + " | " +
                                entry.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:ss"));
                        }

                        break;
                    }

                case "2":
                    {
                        Console.Write("\nВведите имя сохранения: ");
                        string name = Console.ReadLine();

                        SaveEntry entry = FindByName(entries.ToList(), name);

                        if (entry.Name == null)
                        {
                            Console.WriteLine("Сохранение не найдено!");
                            continue;
                        }

                        SaveData save = LoadSave(_savesDirectory, entry);

                        if (save == null)
                        {
                            continue;
                        }

                        PrintSave(entry, save);

                        break;
                    }

                case "3":
                    {
                        Console.Write("\nВведите имя сохранения: ");
                        string name = Console.ReadLine();

                        SaveEntry entry = FindByName(entries.ToList(), name);

                        if (entry.Name == null)
                        {
                            Console.WriteLine("Сохранение не найдено!");
                            continue;
                        }

                        SaveData save = LoadSave(_savesDirectory, entry);

                        if (save == null)
                        {
                            continue;
                        }

                        Console.WriteLine("\nТекущие данные:");
                        PrintSave(entry, save);

                        Console.Write("\nВведите новую карту или оставьте пусто: ");
                        string mapBuffer = Console.ReadLine();

                        if (mapBuffer != "")
                        {
                            save.Map = mapBuffer;
                        }

                        Console.Write("Введите новую координату X или оставьте пусто: ");
                        string xBuffer = Console.ReadLine();

                        if (xBuffer != "")
                        {
                            if (int.TryParse(xBuffer, out int x))
                            {
                                save.Position.X = x;
                            }
                            else
                            {
                                Console.WriteLine("Некорректная координата X!");
                            }
                        }

                        Console.Write("Введите новую координату Y или оставьте пусто: ");
                        string yBuffer = Console.ReadLine();

                        if (yBuffer != "")
                        {
                            if (int.TryParse(yBuffer, out int y))
                            {
                                save.Position.Y = y;
                            }
                            else
                            {
                                Console.WriteLine("Некорректная координата Y!");
                            }
                        }

                        Console.Write("Введите новое здоровье или оставьте пусто: ");
                        string healthBuffer = Console.ReadLine();

                        if (healthBuffer != "")
                        {
                            if (int.TryParse(healthBuffer, out int health))
                            {
                                save.Health = health;
                            }
                            else
                            {
                                Console.WriteLine("Некорректное значение здоровья!");
                            }
                        }

                        Console.WriteLine("Введите новый инвентарь через запятую или оставьте пусто:");
                        string inventoryBuffer = Console.ReadLine();

                        if (inventoryBuffer != "")
                        {
                            string[] inventoryItems = inventoryBuffer.Split(',');

                            save.Inventory.Clear();

                            foreach (string item in inventoryItems)
                            {
                                save.Inventory.Add(item.Trim());
                            }
                        }

                        SaveGame(_savesDirectory, entry, save);

                        for (int i = 0; i < entries.Length; i++)
                        {
                            if (entries[i].Name == entry.Name)
                            {
                                entries[i].UpdatedAt = DateTime.Now;
                                break;
                            }
                        }

                        SaveIndex(_indexPath, entries);

                        Console.WriteLine("Сохранение изменено!");

                        break;
                    }

                case "4":
                    {
                        Console.Write("\nВведите имя сохранения: ");
                        string name = Console.ReadLine();

                        SaveEntry entry = FindByName(entries.ToList(), name);

                        if (entry.Name == null)
                        {
                            Console.WriteLine("Сохранение не найдено!");
                            continue;
                        }

                        CreateBackup(_savesDirectory, _backupsDirectory, entry);

                        Console.WriteLine("Резервная копия создана");

                        break;
                    }

                case "5":
                    {
                        Console.Write("\nВведите имя сохранения: ");
                        string name = Console.ReadLine();

                        SaveEntry entry = FindByName(entries.ToList(), name);

                        if (entry.Name == null)
                        {
                            Console.WriteLine("Сохранение не найдено!");
                            continue;
                        }

                        RestoreBackup(_savesDirectory, _backupsDirectory, entry);

                        break;
                    }

                case "6":
                    {
                        Console.Write("\nВведите имя сохранения: ");
                        string name = Console.ReadLine();

                        SaveEntry entry = FindByName(entries.ToList(), name);

                        if (entry.Name == null)
                        {
                            Console.WriteLine("Сохранение не найдено!");
                            continue;
                        }

                        DeleteSave(_savesDirectory, _indexPath, entry);

                        entries = LoadIndex(_indexPath);

                        Console.WriteLine("Сохранение удалено!");

                        break;
                    }

                case "7":
                    {
                        return;
                    }

                default:
                    {
                        break;
                    }
            }
        }
    }
    public static SaveEntry[] LoadIndex(string indexPath)
    {
        string json = File.ReadAllText(indexPath);
        SaveEntry[] saveEntries = JsonConvert.DeserializeObject<SaveEntry[]>(json);
        return saveEntries;
    }
    public static SaveData LoadSave(string savesDirectory, SaveEntry entry)
    {
        string directory = $"{savesDirectory}\\{entry.File}";
        if (!File.Exists(directory))
        {
            Console.WriteLine("Такого файла не существует!");
            return null;
        }
        string json = File.ReadAllText(directory);
        SaveData data = JsonConvert.DeserializeObject<SaveData>(json);
        if (data == null)
        {
            Console.WriteLine("Данные повреждены!");
            return null;
        }
        return data;
    }
    public static void PrintSave(SaveEntry entry, SaveData save)
    {
        Console.WriteLine("\nИмя сохранения: " + entry.Name);
        Console.WriteLine("Персонаж: " + save.Character);
        Console.WriteLine("Карта: " + save.Map);
        Console.WriteLine("Координаты: " + save.Position.X + " " + save.Position.Y);
        Console.WriteLine("Здоровье: " + save.Health);
        Console.WriteLine("Инвентарь: ");
        foreach (string item in save.Inventory)
        {
            Console.Write(item + ", ");
        }
        Console.WriteLine();
    }
    public static SaveEntry FindByName(List<SaveEntry> entries, string name)
    {
        name = name.ToLower();
        foreach (SaveEntry entry in entries)
        {
            if (entry.Name.ToLower() == name)
            {
                return entry;
            }
        }
        return new SaveEntry(); // в задании не было прописано что делать, если поиск не выдал ничего
    }
    public static void SaveIndex(string indexPath, SaveEntry[] entries)
    {
        string json = JsonConvert.SerializeObject(entries, Formatting.Indented);
        File.WriteAllText(indexPath, json);
    }
    public static void SaveGame(string savesDirectory, SaveEntry entry, SaveData save)
    {
        string json = JsonConvert.SerializeObject(save, Formatting.Indented);
        File.WriteAllText(savesDirectory + $"\\{entry.File}", json);
    }
    public static void CreateBackup(string savesDirectory, string backupsDirectory, SaveEntry entry)
    {
        File.Copy(savesDirectory + "\\" + entry.File, backupsDirectory + "\\" + entry.File, true);
    }
    public static void RestoreBackup(string savesDirectory, string backupsDirectory, SaveEntry entry)
    {
        if (!File.Exists(backupsDirectory + "\\" + entry.File))
        {
            Console.WriteLine("Для этого файла бэкапа не существует!");
            return;
        }

        SaveData data = LoadSave(backupsDirectory, entry);
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(savesDirectory + "\\" + entry.File, json);
    }
    public static void DeleteSave(string savesDirectory, string indexPath, SaveEntry entry)
    {
        List<SaveEntry> entries = LoadIndex(indexPath).ToList<SaveEntry>();
        foreach (SaveEntry elem in entries)
        {
            if (elem == entry)
            {
                if (File.Exists(savesDirectory + "\\" + entry.File))
                {
                    File.Delete(savesDirectory + "\\" + entry.File);
                }
                entries.Remove(elem);
                SaveIndex(indexPath, entries.ToArray());
                return;
            }
        }
    }
}
public class Vector2Int
{
    public int X;
    public int Y;

    public Vector2Int(int x, int y)
    {
        X = x;
        Y = y;
    }
}
public struct SaveEntry
{
    public string Name;
    public string File;
    public DateTime UpdatedAt;

    public SaveEntry(string name, string file, DateTime updatedAt)
    {
        Name = name;
        File = file;
        UpdatedAt = updatedAt;
    }
    public static bool operator ==(SaveEntry entry1, SaveEntry entry2)
    {
        if (entry1.Name != entry2.Name)
        {
            return false;
        }
        if (entry1.File != entry2.File)
        {
            return false;
        }
        if (entry1.UpdatedAt != entry2.UpdatedAt)
        {
            return false;
        }
        return true;
    }
    public static bool operator !=(SaveEntry entry1, SaveEntry entry2)
    {
        if (entry1.Name == entry2.Name)
        {
            return false;
        }
        if (entry1.File == entry2.File)
        {
            return false;
        }
        if (entry1.UpdatedAt == entry2.UpdatedAt)
        {
            return false;
        }
        return true;
    }
}
public class SaveData
{
    public string Character { get; set; }
    public string Map { get; set; }
    public Vector2Int Position { get; set; }
    public int Health { get; set; }
    public List<string> Inventory { get; set; }

    public SaveData(string character, string map, Vector2Int position, int health, List<string> inventory)
    { 
        Character = character;
        Map = map;
        Position = position;
        Health = health;
        Inventory = inventory;
    }
}
