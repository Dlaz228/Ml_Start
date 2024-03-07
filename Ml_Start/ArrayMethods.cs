using Ml_Start.ConfigurationLibrary;


namespace Ml_Start.GenerateSomeNumber;

public class ArrayMethods
{
    public void FillOddNums(int[] array, int startValue)
    {
        int n = startValue;

        for (int i = 0; i < array.Length; i++)
        {
            array[i] = n;

            n += 2;
        }
    }

    public void FillRandomDoubleNums(double[] array, double minValue, double maxValue)
    {
        for (int i = 0; i < 13; i++)
        {
            Random random = new();

            array[i] = Math.Round(minValue + random.NextDouble() * (maxValue - minValue), 13);

            //Log.Information($"Используется неявное приведение типа int в double, и значение записывается в элемент x[{i}]");
            //LoggingTools.WriteLog("Information", $"Используется неявное приведение типа int в double, и значение записывается в элемент x[{i}]");
        }
    }

    public void FillArray(int[] k, double[] x, double[,] k2)
    {
        int[] tmp = { 5, 7, 11, 15 };

        for (int i = 0; i < k.Length - 1; i++)
        {
            for (int j = 0; j < x.Length - 1; j++)
            {
                if (k[i] == 9)
                {
                    k2[i, j] = Math.Sin(Math.Sin(Math.Pow(x[j] / (x[j] + 0.5), x[j])));

                    if (double.IsNaN(k2[i, j]))
                    {
                        //Log.Warning($"В результате вычислений элементу k2[{i}, {j}] было присвоено NaN");
                        LoggingTools.WriteLog("Warning", $"В результате вычислений элементу k2[{i}, {j}] было присвоено NaN");

                    }
                }

                else if (Array.IndexOf(tmp, k[i]) != -1)
                {
                    double expression = 0.5 / (Math.Tan(2 * x[j]) + 2.0 / 3.0);
                    k2[i, j] = Math.Pow(expression, Math.Pow(Math.Pow(x[j], 1.0 / 3.0), 1.0 / 3.0)); ;

                    if (double.IsNaN(k2[i, j]))
                    {
                        //Log.Warning($"В результате вычислений элементу k2[{i}, {j}] было присвоено NaN");
                        LoggingTools.WriteLog("Warning", $"В результате вычислений элементу k2[{i}, {j}] было присвоено NaN");
                    }
                }

                else
                {
                    k2[i, j] = Math.Tan(Math.Pow(Math.Pow(Math.E, 1 - x[j] / Math.PI) / 3.0 / 4.0, 3.0));

                    if (double.IsNaN(k2[i, j]))
                    {
                        //Log.Warning($"В результате вычислений элементу k2[{i}, {j}] было присвоено NaN");
                        LoggingTools.WriteLog("Warning", $"В результате вычислений элементу k2[{i}, {j}] было присвоено NaN");
                    }
                }
            }
        }
    }

    public double GetMinValue(int N, double[,] k2)
    {
        double[] subArray1 = Enumerable.Range(0, k2.GetLength(1))
                            .Select(col => k2[N % 8, col])
                            .ToArray();

        return subArray1.Min();
    }

    public double GetAverageValue(int L, double[,] k2)
    {
        double[] subArray2 = Enumerable.Range(0, k2.GetLength(1))
                           .Select(col => k2[L % 13, col])
                           .ToArray();

        return subArray2.Average();

    }
}
