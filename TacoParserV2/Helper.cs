using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacoParserV2;

public static class Helper
{
    public static string GetResponse(string text)
    {
        Console.Clear();
        Console.Write(text);
        return Console.ReadLine();
    }
}
