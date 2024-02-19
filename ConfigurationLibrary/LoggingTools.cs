using Serilog;
using Serilog.Events;

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
                        .WriteTo.File(tmp.Value));
        }

        Log.Logger = loggerConfig.CreateLogger();
    }

    public static void WriteLog(string logType, string message)
    {
        switch (logType)
        {
            case "Information":
                Log.Information(message);
                return;
            case "Warning":
                Log.Warning(message);
                return;
            case "Debug":
                Log.Debug(message);
                return;
            case "Error":
                Log.Error(message);
                return;
        }
    }
}
