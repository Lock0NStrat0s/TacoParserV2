using TacoParserV2.ApplicationManager;

namespace TacoParserV2;

class Program
{
    static void Main(string[] args)
    {
        Application.LoadEnv();

        bool isRunning = true;
        do
        {
            isRunning = Application.RunApplication();

            Console.Write("\nPress any key to continue: ");
            Console.ReadKey();
        } while (isRunning);
    }
}
