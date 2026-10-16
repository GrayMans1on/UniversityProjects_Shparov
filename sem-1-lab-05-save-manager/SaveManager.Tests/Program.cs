using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SaveManager;

[TestFixture]
public class SaveManagerTests
{
    private string _testDirectory;
    private string _indexPath;
    private string _savesDirectory;
    private string _backupsDirectory;

    [SetUp]
    public void SetUp()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), "SaveManagerTests-" + Guid.NewGuid());
        _indexPath = Path.Combine(_testDirectory, "index.json");
        _savesDirectory = Path.Combine(_testDirectory, "saves");
        _backupsDirectory = Path.Combine(_testDirectory, "backups");

        Directory.CreateDirectory(_savesDirectory);
        Directory.CreateDirectory(_backupsDirectory);

        File.WriteAllText(_indexPath,
            "[{\"name\":\"slot-01\",\"file\":\"save-01.json\",\"updatedAt\":\"2026-04-18T10:15:00\"}]");
        File.WriteAllText(Path.Combine(_savesDirectory, "save-01.json"),
            "{\"character\":\"Mira\",\"map\":\"Ashen Coast\",\"position\":{\"x\":128,\"y\":47},\"health\":76,\"inventory\":[\"iron sword\"]}");
    }

    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }

    [Test]
    public void LoadIndex_ReadsSaveEntry()
    {
        SaveEntry[] entries = Program.LoadIndex(_indexPath);
        //Assert.That(new string[]{ _indexPath[^50..], _savesDirectory[^50..], _backupsDirectory[^50..] }, Is.EqualTo(new string[] { }));

        Assert.That(entries, Has.Length.EqualTo(1));
        Assert.That(entries[0].Name, Is.EqualTo("slot-01"));
        Assert.That(entries[0].File, Is.EqualTo("save-01.json"));
    }

    [Test]
    public void FindByName_IsCaseInsensitive()
    {
        SaveEntry[] entries = Program.LoadIndex(_indexPath);

        SaveEntry entry = Program.FindByName(entries.ToList<SaveEntry>(), "SLOT-01");

        Assert.That(entry, Is.Not.Null);
        Assert.That(entry.Name, Is.EqualTo("slot-01"));
    }

    [Test]
    public void FindByName_ReturnsEmptyWhenSaveDoesNotExist()
    {
        SaveEntry[] entries = Program.LoadIndex(_indexPath);

        Assert.That(Program.FindByName(entries.ToList<SaveEntry>(), "missing").Name, Is.Null);
    }

    [Test]
    public void LoadSave_ReadsCharacterAndWorldData()
    {
        SaveEntry entry = Program.LoadIndex(_indexPath)[0];

        SaveData save = Program.LoadSave(_savesDirectory, entry);

        Assert.That(save.Character, Is.EqualTo("Mira"));
        Assert.That(save.Map, Is.EqualTo("Ashen Coast"));
        Assert.That(save.Position.X, Is.EqualTo(128));
        Assert.That(save.Position.Y, Is.EqualTo(47));
        Assert.That(save.Health, Is.EqualTo(76));
        Assert.That(save.Inventory, Does.Contain("iron sword"));
    }

    [Test]
    public void SaveGame_WritesEditedDataThatCanBeLoadedAgain()
    {
        SaveEntry entry = Program.LoadIndex(_indexPath)[0];
        SaveData save = Program.LoadSave(_savesDirectory, entry);
        save.Map = "Crystal Cavern";
        save.Position.X = 10;
        save.Position.Y = 25;
        save.Health = 50;
        save.Inventory.Add("crystal key");

        Program.SaveGame(_savesDirectory, entry, save);
        SaveData loaded = Program.LoadSave(_savesDirectory, entry);

        Assert.That(loaded.Map, Is.EqualTo("Crystal Cavern"));
        Assert.That(loaded.Position.X, Is.EqualTo(10));
        Assert.That(loaded.Position.Y, Is.EqualTo(25));
        Assert.That(loaded.Health, Is.EqualTo(50));
        Assert.That(loaded.Inventory, Does.Contain("crystal key"));
    }

    [Test]
    public void SaveIndex_WritesEntriesThatCanBeLoadedAgain()
    {
        List<SaveEntry> entries = Program.LoadIndex(_indexPath).ToList<SaveEntry>();
        entries.Add(new SaveEntry
        {
            Name = "manual",
            File = "manual.json",
            UpdatedAt = new DateTime(2026, 5, 1, 12, 30, 0)
        });

        Program.SaveIndex(_indexPath, entries.ToArray());
        SaveEntry[] loaded = Program.LoadIndex(_indexPath);

        Assert.That(loaded, Has.Length.EqualTo(2));
        Assert.That(loaded[1].Name, Is.EqualTo("manual"));
        Assert.That(loaded[1].UpdatedAt, Is.EqualTo(new DateTime(2026, 5, 1, 12, 30, 0)));
    }

    [Test]
    public void CreateBackup_CopiesSaveFile()
    {
        SaveEntry entry = Program.LoadIndex(_indexPath)[0];

        Program.CreateBackup(_savesDirectory, _backupsDirectory, entry);

        string backupPath = Path.Combine(_backupsDirectory, entry.File);
        Assert.That(File.Exists(backupPath), Is.True);
        Assert.That(File.ReadAllText(backupPath), Is.EqualTo(File.ReadAllText(Path.Combine(_savesDirectory, entry.File))));
    }

    [Test]
    public void RestoreBackup_ReplacesCurrentSaveWithBackup()
    {
        SaveEntry entry = Program.LoadIndex(_indexPath)[0];
        Program.CreateBackup(_savesDirectory, _backupsDirectory, entry);
        File.WriteAllText(Path.Combine(_savesDirectory, entry.File), "changed");

        Program.RestoreBackup(_savesDirectory, _backupsDirectory, entry);

        Assert.That(File.ReadAllText(Path.Combine(_savesDirectory, entry.File)),
            Does.Contain("Ashen Coast"));
    }

    [Test]
    public void DeleteSave_RemovesFileAndIndexEntry()
    {
        SaveEntry[] entries = Program.LoadIndex(_indexPath);
        SaveEntry entry = entries[0];

        Program.DeleteSave(_savesDirectory, _indexPath, entry);

        Assert.That(File.Exists(Path.Combine(_savesDirectory, entry.File)), Is.False);
        Assert.That(Program.LoadIndex(_indexPath), Is.Empty);
    }
}