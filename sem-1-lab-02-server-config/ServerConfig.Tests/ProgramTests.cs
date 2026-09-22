using ServerConfig;
using NUnit.Framework;

namespace ServerConfig.Tests;

public class ProgramTests
{
    [Test]
    public void CheckConfiguration_ValidConfiguration_ServerIsReady()
    {
        var result = Program.CheckConfiguration(50, 8, true, false);

        Assert.That(result, Is.EqualTo("Сервер готов к запуску."));
    }

    [Test]
    public void CheckConfiguration_ZeroPlayers_LaunchIsImpossible()
    {
        var result = Program.CheckConfiguration(0, 8, true, false);

        Assert.That(
            result,
            Is.EqualTo("Запуск невозможен: количество игроков должно быть больше нуля."));
    }

    [Test]
    public void CheckConfiguration_NotEnoughMemory_LaunchIsImpossible()
    {
        var result = Program.CheckConfiguration(50, 1, true, false);

        Assert.That(
            result,
            Is.EqualTo("Запуск невозможен: серверу недостаточно оперативной памяти."));
    }

    [Test]
    public void CheckConfiguration_PublicServerWithPassword_LaunchWithWarning()
    {
        var result = Program.CheckConfiguration(50, 8, true, true);

        Assert.That(
            result,
            Is.EqualTo("Запуск возможен с предупреждением: публичный сервер защищён паролем."));
    }

    [Test]
    public void CheckConfiguration_TooManyPlayersForAvailableMemory_LaunchWithWarning()
    {
        var result = Program.CheckConfiguration(150, 4, true, false);

        Assert.That(
            result,
            Is.EqualTo(
                "Запуск возможен с предупреждением: для такого количества игроков рекомендуется больше оперативной памяти."));
    }

    [Test]
    public void CheckConfiguration_InvalidPlayersAndPublicServerWithPassword_LaunchIsImpossible()
    {
        var result = Program.CheckConfiguration(0, 8, true, true);

        Assert.That(
            result,
            Is.EqualTo("Запуск невозможен: количество игроков должно быть больше нуля."));
    }

    [Test]
    public void CheckConfiguration_DisabledAnticheat_LaunchWithAntiCheatMessage()
    {
        var result = Program.CheckConfiguration(50, 8, false, false);

        Assert.That(
            result,
            Is.EqualTo("Сервер готов к запуску. Анти-чит будет выключен."));
    }
}
