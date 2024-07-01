using TacoParserV2.Models;

namespace TacoParserV2.Logger;

public class TacoLogger : ILog
{
    public void LogFatal(string log, Exception exception = null)
    {
        Console.WriteLine($"Fatal: {log}, Exception {exception}");
    }

    public void LogError(string log, Exception exception = null)
    {
        Console.WriteLine($"Error: {log}, Exception {exception}");
    }

    public void LogWarning(string log)
    {
        Console.WriteLine($"Warning: {log}");
    }

    public void LogInfo(string log)
    {
        Console.WriteLine($"Info: {log}");
    }

    public void LogDebug(string log)
    {
        Console.WriteLine($"Debug: {log}");
    }

    public void LogResults(TacoBellLocation coordinates)
    {
        Random random = new Random();
        Console.Beep(5000, 500);
        Console.ForegroundColor = (random.Next(1, 11)) switch
        {
            1 => ConsoleColor.Red,
            2 => ConsoleColor.White,
            3 => ConsoleColor.Blue,
            4 => ConsoleColor.Yellow,
            5 => ConsoleColor.Magenta,
            6 => ConsoleColor.Cyan,
            7 => ConsoleColor.DarkBlue,
            8 => ConsoleColor.DarkGreen,
            9 => ConsoleColor.DarkMagenta,
            10 => ConsoleColor.DarkYellow,
            _ => ConsoleColor.Green
        };
        Console.WriteLine($"Latitude: {coordinates.Location.Latitude}\tLongitude: {coordinates.Location.Longitude}\nAddress: {coordinates.Name}\n\n");
    }

    public void LogResults(string log)
    {
        Random random = new Random();
        Console.ForegroundColor = (random.Next(1, 11)) switch
        {
            1 => ConsoleColor.Red,
            2 => ConsoleColor.White,
            3 => ConsoleColor.Blue,
            4 => ConsoleColor.Yellow,
            5 => ConsoleColor.Magenta,
            6 => ConsoleColor.Cyan,
            7 => ConsoleColor.DarkBlue,
            8 => ConsoleColor.DarkGreen,
            9 => ConsoleColor.DarkMagenta,
            10 => ConsoleColor.DarkYellow,
            _ => ConsoleColor.Green
        };

        Console.WriteLine(log);
    }
}
