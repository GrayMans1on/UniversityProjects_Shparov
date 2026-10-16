using Newtonsoft.Json;

namespace SaveManager;
public class Program
{
    static void Main()
    {
        SaveEntry[] entries = LoadIndex("..\\..\\..\\index.json");
        Console.WriteLine(entries[1].UpdatedAt);

        SaveData newSave = new SaveData("Megachad", "Tilt Towers", new Vector2Int(15, 0), 150, new List<string>() { "Huita", "Shit" });
        SaveEntry newEntry = new SaveEntry("save-03", "save-03.json", DateTime.Parse("2026-12-12T08:00:00"));
        SaveGame("..\\..\\..\\saves", newEntry, newSave);
        //DeleteSave("..\\..\\..\\saves", "..\\..\\..\\index.json", newEntry);
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
