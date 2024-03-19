using Serilog;
using Serilog.Events;
using System.Diagnostics;

namespace Ml_Start.ConfigurationLibrary;

public class LoggingTools
{
    public static void CreateLogger()
    {
        Dictionary<LogEventLevel, string> levels = new Dictionary<LogEventLevel, string>()
{
        { LogEventLevel.Information, "infoLog.txt" },
        { LogEventLevel.Debug, "debugLog.txt" },
        { LogEventLevel.Warning, "warningsLog.txt" },
        { LogEventLevel.Error, "errorsLog.txt" }
};

        LoggerConfiguration loggerConfig = new LoggerConfiguration()
            .MinimumLevel.Verbose();

        foreach (var tmp in levels)
        {
            loggerConfig
                .WriteTo.Logger(c =>
                    c.Filter.ByIncludingOnly(e => e.Level == tmp.Key)
                        .WriteTo.File($"Logs/" + tmp.Value));
        }

        Log.Logger = loggerConfig.CreateLogger();
    }

    public static void WriteLog(string logType, string message, Exception? ex = null)
    {
        switch (logType)
        {
            case "Information":
                Log.Information(message);
                return;
            case "Warning":
                if (ex != null)
                {
                    Log.Warning($"{message}; Sourse: {ex.Source}; warning message: '{ex.Message}'; Where:{ex.StackTrace.Split('\n').Last()}");
                }
                else
                {
                    Log.Warning(message);
                }
                return;
            case "Debug":
                Log.Debug(message);
                return;
            case "Error":
                Log.Error($"{message}; Sourse: {ex.Source}; error message: '{ex.Message}'; Where:{ex.StackTrace.Split('\n').Last()}");
                return;
        }
    }
}
