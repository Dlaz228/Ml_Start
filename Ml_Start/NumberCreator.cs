using Serilog;
using Ml_Start.ConfigurationLibrary;

namespace Ml_Start.GenerateSomeNumber;

public class NumberCreator
{
    public double GetNumber()
    {
        int N;
        int L;

        ArrayMethods arrayMethods = new();
        CongfigTools congfigTools = new();

        LoggingTools.CreateLogger();

        //otherMethods.CreateConfigFile();
        congfigTools.CreateConfigXmlFile();

        int[] k = new int[8];
        arrayMethods.FillOddNums(k, 5);

        double[] x = new double[13];
        arrayMethods.FillRandomDoubleNums(x, -12.0, 15.0);

        double[,] k2 = new double[8, 13];
        arrayMethods.FillArray(k, x, k2);

        congfigTools.GetVariables(out N, out L);

        double element = Math.Round(arrayMethods.GetAverageValue(L, k2) + arrayMethods.GetMinValue(N, k2), 4);

        if (double.IsNaN(element))
        {
            Log.Warning($"В результате вычислений переменной element было присвоено NaN\"");
        }

        return element;
    }
}
