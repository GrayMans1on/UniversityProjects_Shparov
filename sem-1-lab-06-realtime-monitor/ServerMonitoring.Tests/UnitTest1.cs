using NUnit.Framework;
using ServerMonitoring;
using System;
using System.Collections.Generic;
using System.IO;

namespace ServerMonitoring.Tests;

public class MonitorTests
{
    [Test]
    public void ToKelvin_ConvertsCorrectly()
    {
        float result = Program.ToKelvin(0);

        Assert.That(result, Is.EqualTo(273));
    }

    [Test]
    public void ToFahrenheit_ConvertsCorrectly()
    {
        float result = Program.ToFahrenheit(0);

        Assert.That(result, Is.EqualTo(32));
    }

    [Test]
    public void PrintGraph_PrintsGraphName()
    {
        StringWriter output = new StringWriter();
        Console.SetOut(output);

        List<float> values = new List<float>() { 10, 20, 30 };

        Program.PrintGraph("CPU, %", values, 0, 100);

        string result = output.ToString();

        Assert.That(result, Does.Contain("CPU, %"));
    }

    [Test]
    public void PrintGraph_PrintsAllValuesAsColumns()
    {
        StringWriter output = new StringWriter();
        Console.SetOut(output);

        List<float> values = new List<float>() { 10, 50, 100 };

        Program.PrintGraph("CPU, %", values, 0, 100);

        string result = output.ToString();

        Assert.That(result, Does.Contain("█"));
    }

    [Test]
    public void PrintGraph_HasCorrectHeight()
    {
        StringWriter output = new StringWriter();
        Console.SetOut(output);

        List<float> values = new List<float>() { 50 };

        Program.PrintGraph("CPU, %", values, 0, 100);

        string[] lines = output.ToString()
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        Assert.That(lines.Length, Is.EqualTo(12));
    }

    [Test]
    public void PrintCurrentData_PrintsTimestamp()
    {
        StringWriter output = new StringWriter();
        Console.SetOut(output);

        DateTime timestamp = new DateTime(2026, 10, 24, 12, 30, 15, 500);

        Program.PrintCurrentData(50, 60, 70, timestamp);

        string result = output.ToString();

        Assert.That(result, Does.Contain("24.10.2026 12:30:15.50"));
    }

    [Test]
    public void PrintCurrentData_PrintsCpuAndRam()
    {
        StringWriter output = new StringWriter();
        Console.SetOut(output);

        Program.PrintCurrentData(
            25.5f,
            75.5f,
            50,
            DateTime.Now);

        string result = output.ToString();

        Assert.That(result, Does.Contain("25,5 %"));
        Assert.That(result, Does.Contain("75,5 %"));
    }

    [Test]
    public void AddMeasurement_StoresOnlyTwentyMeasurements()
    {
        for (int i = 0; i < 25; i++)
        {
            Program.AddMeasurement(
                new ServerMeasurement(i, i, i, DateTime.Now));
        }

        Assert.That(Program.Measurements.Count, Is.EqualTo(20));
    }
}