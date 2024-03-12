using Serilog;
using System.Xml.Linq;

namespace Ml_Start.ConfigurationLibrary;

public class CongfigTools
{
    public static void CreateServerConfigXmlFile()
    {
        try
        {
            XDocument doc = XDocument.Load("config.xml");
        }
        catch
        {
            XDocument xdoc = new XDocument(
            new XElement("ConfigSettings",
                new XElement("ConnectionString", "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MlStartDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False")
                )
            );

            xdoc.Save("config.xml");

            Log.Information("Конфигурационный файл создан!");
        }
    }

    public static void CreateClientConfigXmlFile()
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
                new XElement("Delay", "5000")
                )
            );

            xdoc.Save("config.xml");

            Log.Information("Конфигурационный файл создан!");
        }
    }

    public static void GetVariables(out int N, out int L)
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

    public static string GetVariableFromXml(string name)
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
    }
}
