using System.Net.Sockets;
using System.Net;
using Ml_Start.ConfigurationLibrary;
using Ml_Start.GenerateSomeNumber;
using Ml_Start.MakeStory;
using Server;
using static Server.HelpingFunctions;

internal class Program
{
    static List<string> users = new();

    public static void Main()
    {
        TcpListener listener = null;
        IPAddress defaultIpAddress = IPAddress.Parse("127.0.0.1"); //"127.0.0.1"
        int defaultPort = 8080; //8080

        try
        {
            CongfigTools.CreateServerConfigXmlFile();
            LoggingTools.CreateLogger();

            CreateDataBase();

            GetCustomIpAndPort(ref defaultIpAddress, ref defaultPort);

            Console.Clear();
            //ShowServerNetworkConfig();
            listener = new(defaultIpAddress, defaultPort);
            listener.Start();
            WriteToConsole("MultiIPEchoServer started...");
            WriteToConsole($"IP => {defaultIpAddress}; Port => {defaultPort}");
            LoggingTools.WriteLog("Information", "Server has started");
            WriteToConsole("Waiting for incoming client connections...");
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                WriteToConsole($"Accepted new client({client.Client.RemoteEndPoint}) connection...");
                LoggingTools.WriteLog("Information", $"Accepted new client({client.Client.RemoteEndPoint}) connection...");
                Thread t = new(ProcessClientRequests);
                t.Start(client);
            }
        }
        catch (Exception ex)
        {
            WriteToConsole("Произошла ошибка");
            LoggingTools.WriteLog("Error", "Произошла ошибка на сервере", ex);
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
                string clientMessage = reader.ReadLine();
                //WriteToConsole($"Message from {name}({client.Client.RemoteEndPoint}) -> {clientMessage}");

                if (clientMessage.Equals("Reg"))
                {
                    string login = reader.ReadLine();
                    string password = reader.ReadLine();
                    DBRegistration.UserRegistration(login, password, client);
                }
                else if (clientMessage.Equals("Auth"))
                {
                    string login = reader.ReadLine();
                    string password = reader.ReadLine();
                    string isAuth = DBAuthorization.UserAuthorization(login, password, users, client, ref name);

                    if (isAuth.Equals("true") && !users.Contains(login))
                    {
                        users.Add(login);
                    }

                    writer.WriteLine(isAuth);
                }
                else if (clientMessage.Equals("Story"))
                {
                    while (!clientMessage.Equals("close"))
                    {
                        int N = reader.ReadLine().Length;
                        Console.WriteLine(N);
                        int L = reader.ReadLine().Length;
                        Console.WriteLine(L);

                        foreach (string line in story.GetStory(someNumber.GetNumber(N, L)))
                        {
                            WriteToConsole($"For {name}({client.Client.RemoteEndPoint}) -> " + line);
                            writer.WriteLine(line);

                            clientMessage = reader.ReadLine();
                            int delay = int.Parse(clientMessage);
                            WriteToConsole($"Delay from client({client.Client.RemoteEndPoint}) -> {delay}");

                            await Task.Delay(delay);
                        }

                        writer.WriteLine("stop");
                        clientMessage = reader.ReadLine();
                        //Console.WriteLine(clientMessage);
                        int delayForNextDay = int.Parse(clientMessage);
                        Console.WriteLine(delayForNextDay);

                        await Task.Delay(delayForNextDay);
                    }
                }
            }

        }
        catch (IOException)
        {
            Disconnect(name, client);
        }
        catch (NullReferenceException)
        {
            Disconnect(name, client);
        }
        catch (Exception ex)
        {
            WriteToConsole("Произошла необработанная ошибка сервера.");
            Console.WriteLine(ex);
            LoggingTools.WriteLog("Error", "Произошла необработанная ошибка сервера.", ex);
        }
        finally
        {
            if (client != null)
            {
                client.Close();
            }
        }
    }

    private static void Disconnect(string name, TcpClient client)
    {
        users.Remove(name);

        WriteToConsole($"Client {name}({client.Client.RemoteEndPoint}) connection closed!");
        LoggingTools.WriteLog("Information", $"Client {name}({client.Client.RemoteEndPoint}) connection closed!");

        client.Close();
    }

    
}
