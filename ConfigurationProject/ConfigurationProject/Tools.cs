using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Serilog;
using Serilog.Events;

namespace Ml_Start.ConfigurationProject;

public class Tools
{
    public void CreateConfigXmlFile()
    {
        try
        {
            XDocument doc = XDocument.Load("config.xml");
        }
        catch
        {
            XDocument xdoc = new XDocument(
            new XElement("ConfigSettings",
                new XElement("FirstName", "Даниил"),
                new XElement("LastName", "Лазукин"),
                new XElement("Delay", "1000")
                )
            );

            xdoc.Save("config.xml");

            Log.Information("Конфигурационный файл создан!");
        }
    }

    public static LoggerConfiguration CreateLoggerConfig()
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

        return loggerConfig;
    }

    public void GetVariables(out int N, out int L)
    {
        try
        {
            N = GetVariableFromXml("FirstName").Length;
            L = GetVariableFromXml("LastName").Length;
        }
        catch
        {
            Log.Error($"Чтение данных из конфигурационного файла вызвало ошибку");

            N = 0;
            L = 0;
        }
    }

    public string GetVariableFromXml(string name)
    {
        XDocument xDoc = XDocument.Load("config.xml");

        var node = xDoc.Descendants(name).Select(element => element.Value);

        return node.First();
    }

    public static void ChangeDelay(string newDelay)
    {
        XDocument xDoc = XDocument.Load("config.xml");

        var delay = xDoc.Element("ConfigSettings");

        delay.Element("Delay").Value = newDelay;

        xDoc.Save("config.xml");

        

        //delay.FirstOrDefault().Value = Convert.ToString(newDelay);
    }

    public static string Hash(string input)
    {
        byte[] byteArray = Encoding.ASCII.GetBytes(input);
        byte[] hash = MD5.HashData(byteArray);

        var sb = new StringBuilder();

        foreach (byte b in hash)
        {
            sb.Append(b.ToString("X2"));
        }

        string hashedString = Convert.ToString(sb) ?? string.Empty;
        if (hashedString == string.Empty)
        {
            Log.Warning("Не удалось совершить хэширование строки");
        }

        return hashedString;
    }
}

