using DotNetEnv;
using GeoCoordinatePortable;
using TacoParserV2.API;
using TacoParserV2.ApplicationManager;
using TacoParserV2.Logger;
using TacoParserV2.Models;

namespace TacoParserV2;

class Program
{
    static void Main(string[] args)
    {
        Application.LoadEnv();

        bool isRunning = true;
        do
        {
            isRunning = Application.RunApplication(isRunning);

            Console.Write("\nPress any key to continue: ");
            Console.ReadKey();
        } while (isRunning);
    }
}
