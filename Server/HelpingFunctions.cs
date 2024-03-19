using Ml_Start.ConfigurationLibrary;
using System.Net.Sockets;
using System.Net;
using Server.MlStartDB;

namespace Server
{
    public class HelpingFunctions
    {
        public static void WriteToConsole(string message, bool isNextLine = true)
        {
            if (isNextLine)
            {
                Console.WriteLine($"[{DateTime.Now}]: {message}");
            }
            else
            {
                Console.Write($"[{DateTime.Now}]: {message}");
            }
        }

        public static void CreateDataBase()
        {
            var context = new MlStartDbContext();

            if (context.Database.EnsureCreated())
            {
                WriteToConsole("База данных создана");
                LoggingTools.WriteLog("Debug", "База данных создана");
            }
        }

        public static void GetCustomIpAndPort(ref IPAddress ipAddress, ref int port)
        {
            string ans = "";

            while (!ans.Equals("Y") && !ans.Equals("N"))
            {
                WriteToConsole("Хотите использовать свой ip и port для сервера? Y/N: ", false);
                ans = Console.ReadLine().Trim().ToUpper();
            }

            if (ans.Equals("Y"))
            {
                while (true)
                {
                    try
                    {
                        WriteToConsole("Введите IP-адресс: ", false);
                        ipAddress = IPAddress.Parse(Console.ReadLine().Trim());
                        WriteToConsole("Введите порт: ", false);
                        port = int.Parse(Console.ReadLine().Trim());

                        if (port < 0 || port > 65535)
                        {
                            throw new FormatException();
                        }

                        TcpListener listener = new(ipAddress, port);
                        listener.Start();
                        listener.Stop();
                        break;
                    }
                    catch (SocketException)
                    {
                        WriteToConsole("Недопустимый IP-адрес");
                    }
                    catch (FormatException)
                    {
                        WriteToConsole("Недопустимый формат");
                    }
                    catch (Exception ex)
                    {
                        WriteToConsole("Произошла ошибка при использовании своего IP и Port");
                        LoggingTools.WriteLog("Error", "Произошла ошибка при использовании своего IP и Port", ex);
                    }
                    finally
                    {
                        Console.WriteLine();
                        Console.WriteLine();
                        //Console.Clear();
                    }
                }
            }
        }
    }
}
