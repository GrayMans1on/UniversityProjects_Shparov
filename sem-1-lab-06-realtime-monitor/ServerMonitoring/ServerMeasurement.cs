public struct ServerMeasurement
{
    public float Cpu;
    public float Ram;
    public float Temperature;
    public DateTime Timestamp;

    public ServerMeasurement(float cpu, float ram, float temperature, DateTime timestamp)
    {
        Cpu = cpu;
        Ram = ram;
        Temperature = temperature;
        Timestamp = timestamp;
    }
}