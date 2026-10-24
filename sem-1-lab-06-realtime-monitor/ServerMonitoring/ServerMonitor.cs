using System;
namespace ServerMonitoring;
public class ServerMonitor
{
    private readonly Random _random = new();

    private readonly System.Timers.Timer _timer;

    private double _cpu = 35;
    private double _ram = 42;
    private double _temperature = 48;

    public event Action<float, float, float, DateTime>? DataReceived;

    public ServerMonitor()
    {
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += (_, _) => GenerateData();
    }

    public void Start()
    {
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    private void GenerateData()
    {
        _cpu += _random.NextDouble() * 10 - 5;
        _ram += _random.NextDouble() * 6 - 3;
        _temperature += _random.NextDouble() * 4 - 2;

        _cpu = Math.Clamp(_cpu, 0, 100);
        _ram = Math.Clamp(_ram, 10, 100);
        _temperature = Math.Clamp(_temperature, 30, 95);

        DataReceived?.Invoke(
            (float)Math.Round(_cpu, 10),
            (float)Math.Round(_ram, 10),
            (float)Math.Round(_temperature, 10),
            DateTime.Now);
    }
}