using Serilog;
using System.Security.Cryptography;
using System.Text;

namespace Ml_Start.ConfigurationLibrary;

public class Hasher
{
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
