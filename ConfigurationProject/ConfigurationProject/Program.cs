using Ml_Start.ConfigurationProject;
using Serilog;

class Program
{
    static void Main()
    {
        LoggerConfiguration loggerConfig = Tools.CreateLoggerConfig();

        Log.Logger = loggerConfig.CreateLogger();

        Log.CloseAndFlush();
    }
}
