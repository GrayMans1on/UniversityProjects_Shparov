namespace ServerMonitoring;

public enum TemperatureType
{
    Celsius,
    Kelvin,
    Fahrenheit
}
public class ServerParameters
{
    private TemperatureType _currentTemperatureType = TemperatureType.Celsius;
    private int _roundDigits = 2;


    public TemperatureType CurrentTemperatureType => _currentTemperatureType;
    public int RoundDigits => _roundDigits;


    public void SetTemperature(TemperatureType type)
    {
        _currentTemperatureType = type;
    }
    public void SetRoundDigits(int digitsCount)
    {
        _roundDigits = Math.Clamp(digitsCount, 0, 10);
    }

    public ServerParameters(TemperatureType temperatureType, int roundDigits)
    {
        _currentTemperatureType = temperatureType;
        _roundDigits = Math.Clamp(roundDigits, 0, 10);
    }
}