using System;
using TacoParserV2.Models;
namespace TacoParserV2.Logger
{
    public interface ILog
    {
        void LogFatal(string log, Exception exception = null);
        void LogError(string log, Exception exception = null);
        void LogWarning(string log);
        void LogInfo(string log);
        void LogDebug(string log);
        void LogResults(TacoBellLocation coordinates);
    }
}
