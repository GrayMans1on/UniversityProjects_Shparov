using ReportGenerator;
using NUnit.Framework;

namespace ReportGenerator.Tests;

public class ProgramTests
{
    [Test]
    public void CreateReportFromString_ValidData_ReturnsFormattedReport()
    {
        string result = Program.CreateReportFromString(
            "TT | CC | DD | dd | UU | uu | PP | WW | RR | MM | pp.",
            "Game Jam", "Финал", "2026-01-01", "2026-01-03", "50", "User01", "9", "10", "5000");

        Assert.That(
            result,
            Is.EqualTo("Game Jam | Финал | 01.01.26 | 03.01.26 | 2 | 48 | 50 | User01 | 9 | 10 | 5000."));
    }

    [Test]
    public void CreateReportFromString_EmptyTitle_UsesDefaultTitle()
    {
        string result = Program.CreateReportFromString(
            "TT.",
            "", "Комментарий", "2026-01-01", "2026-01-02", "10", "User01", "9", "10", "1000");

        Assert.That(result, Is.EqualTo("Событие без названия."));
    }

    [Test]
    public void CreateReportFromString_EmptyComments_UsesDefaultComment()
    {
        string result = Program.CreateReportFromString(
            "CC.",
            "Событие", "", "2026-01-01", "2026-01-02", "10", "User01", "9", "10", "1000");

        Assert.That(result, Is.EqualTo("нет комментария."));
    }

    [Test]
    public void CreateReportFromString_LongComments_TruncatesCommentsTo200Characters()
    {
        string comments = new string('a', 201);

        string result = Program.CreateReportFromString(
            "CC.",
            "Событие", comments, "2026-01-01", "2026-01-02", "10", "User01", "9", "10", "1000");

        Assert.That(result, Is.EqualTo(new string('a', 200) + "."));
    }

    [Test]
    public void CreateReportFromString_ValidDates_CalculatesDuration()
    {
        string result = Program.CreateReportFromString(
            "UU дней | uu часов.",
            "Событие", "Комментарий", "2026-01-01", "2026-01-03", "10", "User01", "9", "10", "1000");

        Assert.That(result, Is.EqualTo("2 дней | 48 часов."));
    }

    [Test]
    public void CreateReportFromString_InvalidStartDate_ReplacesDateAndDurationWithHash()
    {
        string result = Program.CreateReportFromString(
            "DD | UU | uu.",
            "Событие", "Комментарий", "wrong", "2026-01-03", "10", "User01", "9", "10", "1000");

        Assert.That(result, Is.EqualTo("# | # | #."));
    }

    [Test]
    public void CreateReportFromString_InvalidParticipantsCount_ReplacesValueWithHash()
    {
        string result = Program.CreateReportFromString(
            "Участники: PP.",
            "Событие", "Комментарий", "2026-01-01", "2026-01-02", "wrong", "User01", "9", "10", "1000");

        Assert.That(result, Is.EqualTo("Участники: #."));
    }

    [Test]
    public void CreateReportFromString_InvalidWinnerRating_ReplacesValueWithHash()
    {
        string result = Program.CreateReportFromString(
            "RR.",
            "Событие", "Комментарий", "2026-01-01", "2026-01-02", "10", "User01", "wrong", "10", "1000");

        Assert.That(result, Is.EqualTo("#."));
    }

    [Test]
    public void CreateReportFromString_InvalidMaxRating_ReplacesValueWithHash()
    {
        string result = Program.CreateReportFromString(
            "MM.",
            "Событие", "Комментарий", "2026-01-01", "2026-01-02", "10", "User01", "9", "wrong", "1000");

        Assert.That(result, Is.EqualTo("#."));
    }

    [Test]
    public void CreateReportFromString_InvalidPrize_ReplacesValueWithHash()
    {
        string result = Program.CreateReportFromString(
            "pp.",
            "Событие", "Комментарий", "2026-01-01", "2026-01-02", "10", "User01", "9", "10", "wrong");

        Assert.That(result, Is.EqualTo("#."));
    }

    [Test]
    public void CreateReportFromString_AutoFormat_UsesDefaultFormat()
    {
        string result = Program.CreateReportFromString(
            "Auto",
            "Game Jam", "Финал", "2026-01-01", "2026-01-03", "50", "User01", "9", "10", "5000");

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.Contain("Game Jam"));
            Assert.That(result, Does.Contain("Дата начала: 01.01.26"));
            Assert.That(result, Does.Contain("Длительность: 48 часов"));
            Assert.That(result, Does.Contain("Победитель: User01"));
            Assert.That(result, Does.Contain("Победный приз: 5000 руб."));
        });
    }
}