﻿using Serilog;
using System.Xml.Linq;

namespace Ml_Start.ConfigurationLibrary;

public class CongfigTools
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
    }
}