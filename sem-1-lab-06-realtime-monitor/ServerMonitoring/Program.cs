using Newtonsoft.Json;

namespace ServerMonitoring;

public class Program
{
    private static ServerParameters _serverParameters = new ServerParameters(TemperatureType.Celsius, 1);

    private static string _configPath = "..\\..\\..\\config.json";


    private static List<ServerMeasurement> _measurements = new List<ServerMeasurement>();
    private const int MAX_MEASUREMENTS = 20;


    public static List<ServerMeasurement> Measurements => _measurements;
    static void Main()
    {
        ServerMonitor monitor = new ServerMonitor();
        monitor.Start();


        LoadParameters();

        Console.WriteLine("Нажмите Enter, чтобы прервать работу программы...");

        monitor.DataReceived += OnDataReceived;

        Console.ReadLine();
    }
    private static void LoadParameters()
    {
        try
        {
            string json = File.ReadAllText(_configPath);
            _serverParameters = JsonConvert.DeserializeObject<ServerParameters>(json);
        }
        catch (FileNotFoundException e) // ошибка — файла не существует; решение — создать новый файл
        {
            //File.Create(_configPath);

            string json = JsonConvert.SerializeObject(_serverParameters, Formatting.Indented);
            File.WriteAllText(_configPath, json);
        }
        catch (JsonReaderException e) // ошибка — проблемы с чтением Json; решение — перенести текст в другой файл, создать новый
        {
            File.Create(_configPath + ".obsolete");
            File.WriteAllText(_configPath + ".obsolete", File.ReadAllText(_configPath));

            string json = JsonConvert.SerializeObject(_serverParameters, Formatting.Indented);
            File.WriteAllText(_configPath, json);
        }
        catch (Exception e) // остальные ошибки; решение — перезапись файла
        {
            string json = JsonConvert.SerializeObject(_serverParameters, Formatting.Indented);
            File.WriteAllText(_configPath, json);
        }
    }
    public static void AddMeasurement(ServerMeasurement measurement)
    {
        _measurements.Add(measurement);

        if (_measurements.Count > MAX_MEASUREMENTS)
        {
            _measurements.RemoveAt(0);
        }
    }
    public static float ToKelvin(float celsiusTemp)
    {
        return celsiusTemp + 273;
    }

    public static float ToFahrenheit(float celsiusTemp)
    {
        return celsiusTemp * 9F / 5F + 32F;
    }
    private static void OnDataReceived(float cpu, float ram, float temperature, DateTime timestamp)
    {
        AddMeasurement(new ServerMeasurement(cpu, ram, temperature, timestamp));

        Console.Clear();

        PrintCurrentData(cpu, ram, temperature, timestamp);

        Console.WriteLine();
        PrintGraphs();
    }
    public static void PrintGraph(string name, List<float> values, float minValue, float maxValue)
    {
        const int graphHeight = 10;

        Console.WriteLine(name);

        for (int row = graphHeight; row >= 1; row--)
        {
            float level = minValue + (maxValue - minValue) * row / graphHeight;

            Console.Write($"{level,6:F0} |");

            foreach (float value in values)
            {
                if (value >= level)
                {
                    Console.Write("█");
                }
                else
                {
                    Console.Write(" ");
                }
            }

            Console.WriteLine();
        }

        Console.Write("       +");

        for (int i = 0; i < values.Count; i++)
        {
            Console.Write("-");
        }

        Console.WriteLine();
    }
    private static void PrintGraphs()
    {
        List<float> cpuValues = new List<float>();
        List<float> ramValues = new List<float>();
        List<float> temperatureValues = new List<float>();

        foreach (ServerMeasurement measurement in _measurements)
        {
            cpuValues.Add(measurement.Cpu);
            ramValues.Add(measurement.Ram);

            float temperature = measurement.Temperature;

            switch (_serverParameters.CurrentTemperatureType)
            {
                case TemperatureType.Celsius:
                    {
                        break;
                    }

                case TemperatureType.Kelvin:
                    {
                        temperature = ToKelvin(temperature);
                        break;
                    }

                case TemperatureType.Fahrenheit:
                    {
                        temperature = ToFahrenheit(temperature);
                        break;
                    }
            }

            temperatureValues.Add(temperature);
        }

        PrintGraph("CPU, %", cpuValues, 0, 100);

        Console.WriteLine();

        PrintGraph("RAM, %", ramValues, 0, 100);

        Console.WriteLine();

        switch (_serverParameters.CurrentTemperatureType)
        {
            case TemperatureType.Celsius:
                {
                    PrintGraph("Temperature, °C", temperatureValues, 0, 100);
                    break;
                }

            case TemperatureType.Kelvin:
                {
                    PrintGraph("Temperature, K", temperatureValues, 273, 373);
                    break;
                }

            case TemperatureType.Fahrenheit:
                {
                    PrintGraph("Temperature, °F", temperatureValues, 32, 212);
                    break;
                }
        }
    }
    public static void PrintCurrentData(float cpu, float ram, float temperature, DateTime timestamp)
    {
        string temperatureType = "°C";

        switch (_serverParameters.CurrentTemperatureType)
        {
            case TemperatureType.Celsius:
                {
                    temperatureType = "°C";
                    break;
                }

            case TemperatureType.Kelvin:
                {
                    temperature = ToKelvin(temperature);
                    temperatureType = "K";
                    break;
                }

            case TemperatureType.Fahrenheit:
                {
                    temperature = ToFahrenheit(temperature);
                    temperatureType = "°F";
                    break;
                }
        }

        int roundDigits = _serverParameters.RoundDigits;

        cpu = (float)Math.Round(cpu, roundDigits);
        ram = (float)Math.Round(ram, roundDigits);
        temperature = (float)Math.Round(temperature, roundDigits);

        string cpuText = cpu.ToString($"F{roundDigits}") + " %";
        string ramText = ram.ToString($"F{roundDigits}") + " %";
        string temperatureText = temperature.ToString($"F{roundDigits}") + " " + temperatureType;
        string timeText = timestamp.ToString("dd.MM.yyyy HH:mm:ss.ff");

        Console.WriteLine("+------------------------+-------------------+-------------------+----------------------+\n" +
                          "| Timestamp              | CPU               | RAM               | Temp                 |\n" +
                         $"| {timeText,-22} | {cpuText,-17} | {ramText,-17} | {temperatureText,-20} |\n" +
                          "+------------------------+-------------------+-------------------+----------------------+\n");
    }
}
