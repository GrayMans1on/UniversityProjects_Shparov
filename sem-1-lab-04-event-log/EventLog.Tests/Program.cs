using EventLog;
using NUnit.Framework;

[TestFixture]
public class ProgramTests
{
    private LogEntry[] _entries = null!;

    [SetUp]
    public void SetUp()
    {
        string[] lines = File.ReadAllLines("..\\..\\..\\event_server.log");
        _entries = Program.ParseLog(lines);
    }

    [Test]
    public void ParseLog_SplitsTimestampLevelCategoryAndMessage()
    {
        LogEntry first = _entries[0];

        Assert.That(first.Timestamp, Is.EqualTo(new DateTime(2026, 9, 1, 12, 10, 1, 101)));
        Assert.That(first.Level, Is.EqualTo(Level.Info));
        Assert.That(first.Category, Is.EqualTo(Category.Server));
        Assert.That(first.Message, Does.Contain("Событие"));
    }

    [Test]
    public void FilterByDate_ReturnsOnlyMaintenanceDayEntries()
    {
        LogEntry[] result = Program.FilterByDate(_entries, new DateTime(2026, 9, 2));
        Assert.That(result, Has.Length.EqualTo(6));
    }

    [Test]
    public void FilterByLevel_ReturnsAllDatabaseErrors()
    {
        LogEntry[] result = Program.FilterByLevel(_entries, Level.Error);

        Assert.That(result, Has.Length.EqualTo(3));
        Assert.That(result[1].Category, Is.EqualTo(Category.Database));
        Assert.That(result[2].Category, Is.EqualTo(Category.Database));
    }

    [Test]
    public void FilterByCategory_ReturnsOnlyCombatRecords()
    {
        LogEntry[] result = Program.FilterByCategory(_entries, Category.Combat);

        Assert.That(result, Is.Not.Empty);
        Assert.That(result[0].Category, Is.EqualTo(Category.Combat));
    }

    [Test]
    public void Search_IsCaseInsensitive()
    {
        LogEntry[] result = Program.Search(_entries, "сердце эмберфанга");
        Assert.That(result, Has.Length.EqualTo(4));
    }

    [Test]
    public void CountByLevel_CountsFatalRecords()
    {
        Assert.That(Program.CountByLevel(_entries, Level.Fatal), Is.EqualTo(1));
    }

    [Test]
    public void GetServerStatus_ReturnsCriticalWhenFatalServerEntryExists()
    {
        Assert.That(Program.GetServerStatus(_entries),
            Is.EqualTo("КРИТИЧЕСКАЯ ОШИБКА: сервер остановлен"));
    }
}