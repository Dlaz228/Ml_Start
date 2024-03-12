using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using Ml_Start.ConfigurationLibrary;
using Ml_Start.GenerateSomeNumber;
using Ml_Start.MakeStory;
using Server;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Data.SqlClient;
using Server.MlStartDB;

internal class Program
{
    static List<string> users = new();

    private async static void ProcessClientRequests(object argument)
    {
        TcpClient client = (TcpClient)argument;
        string name = "unknown";

        try
        {
            StreamReader reader = new(client.GetStream());
            StreamWriter writer = new(client.GetStream());
            writer.AutoFlush = true;

            //string s = String.Empty;
            //string name = "unknown";

            Story story = new();
            NumberCreator someNumber = new();
            CongfigTools configTools = new();

            while (true)
            {
                string ans = reader.ReadLine();

                //Console.WriteLine(ans);

                if (ans.Equals("Reg"))
                {
                    string login = reader.ReadLine();
                    string password = reader.ReadLine();
                    DBRegistration.UserRegistration(login, password);
                }
                else if (ans.Equals("Auth"))
                {
                    string login = reader.ReadLine();
                    string password = reader.ReadLine();
                    string isAuth = DBAuthorization.UserAuthorization(login, password, users, ref name);

                    if (isAuth.Equals("true") && !users.Contains(login))
                    {
                        users.Add(login);
                    }

                    writer.WriteLine(isAuth);
                }
                else if (ans.Equals("Story"))
                {
                    while (!ans.Equals("close"))
                    {
                        int N = int.Parse(reader.ReadLine());
                        int L = int.Parse(reader.ReadLine());

                        foreach (string line in story.GetStory(someNumber.GetNumber(N, L)))
                        {
                            Console.WriteLine($"From {name} -> " + line);
                            writer.WriteLine(line);
                            //writer.Flush();

                            int delay = int.Parse(reader.ReadLine());
                            //Console.WriteLine(delay);

                            await Task.Delay(delay);

                            //Thread.Sleep(int.Parse(Tools.GetVariableFromXml("Delay")));
                        }

                        writer.WriteLine("stop");
                        ans = reader.ReadLine();

                        if (ans.Equals("Close"))
                        {
                            users.Remove(name);

                            reader.Close();
                            writer.Close();
                            client.Close();
                            Console.WriteLine($"Client {name} connection closed!");
                        }

                        //Console.WriteLine(ans);
                        //break;
                    }

                    break;
                }
                else if (ans.Equals("Close"))
                {
                    users.Remove(name);

                    reader.Close();
                    writer.Close();
                    client.Close();
                    Console.WriteLine($"Client {name} connection closed!");
                    break;
                }
                //else
                //{
                //    LoggingTools.WriteLog("Warning", ans);
                //}

            }
            
        }
        catch (IOException)
        {
            users.Remove(name);

            Console.WriteLine($"Problem with client {name} communication. Exiting thread.");
        }
        finally
        {
            if (client != null)
            {
                client.Close();
            }
        }
    }

    public static void Main()
    {
        TcpListener listener = null;
        IPAddress defaultIpAddress = IPAddress.Parse("127.0.0.1"); //"127.0.0.1"
        int defaultPort = 8080; 

        CongfigTools.CreateServerConfigXmlFile();

        try
        {
            var context = new MlStartDbContext();
            
            if (context.Database.EnsureCreated())
            {
                Console.WriteLine("База данных создана");
                LoggingTools.WriteLog("Debug", "База данных создана");
            }

            LoggingTools.CreateLogger();

            GetCustomIpAndPort(ref defaultIpAddress, ref defaultPort);

            Console.Clear();
            //ShowServerNetworkConfig();
            listener = new(defaultIpAddress, defaultPort);
            listener.Start();
            Console.WriteLine("MultiIPEchoServer started...");
            Console.WriteLine($"IP => {defaultIpAddress}; Port => {defaultPort}");
            LoggingTools.WriteLog("Information", "Server has started");
            Console.WriteLine("Waiting for incoming client connections...");
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Accepted new client connection...");
                LoggingTools.WriteLog("Information", "Accepted new client connection...");
                Thread t = new(ProcessClientRequests);
                t.Start(client);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Произошла ошибка");
            LoggingTools.WriteLog("Error", ex.Message + ex.StackTrace);
        }
        finally
        {
            if (listener != null)
            {
                listener.Stop();
                LoggingTools.WriteLog("Information", "Сервер закрыт");
            }
        }
    }

    private static void GetCustomIpAndPort(ref IPAddress ipAddress, ref int port)
    {
        string ans = "";

        while (!ans.Equals("Y") && !ans.Equals("N"))
        {
            Console.Write("Хотите использовать свой ip и port для сервера? Y/N: ");
            ans = Console.ReadLine().Trim().ToUpper();
        }

        if (ans.Equals("Y"))
        {
            while (true)
            {
                try
                {
                    Console.Write("Введите IP-адресс: ");
                    ipAddress = IPAddress.Parse(Console.ReadLine().Trim());
                    Console.Write("Введите порт: ");
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
                    Console.WriteLine("Недопустимый IP-адрес");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Недопустимый порт");
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
