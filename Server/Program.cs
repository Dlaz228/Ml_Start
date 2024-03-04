using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using Ml_Start.ConfigurationLibrary;
using Ml_Start.GenerateSomeNumber;
using Ml_Start.MakeStory;
using Server;

internal class Program
{
    static List<string> users = new List<string>();

    private async static void ProcessClientRequests(object argument)
    {
        TcpClient client = (TcpClient)argument;
        try
        {
            StreamReader reader = new StreamReader(client.GetStream());
            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.AutoFlush = true;
            string s = String.Empty;
            string name = "unknown";
            Story story = new();
            NumberCreator someNumber = new();
            CongfigTools configTools = new();
            Console.WriteLine($"Пользователь {name} подключился");
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
                    bool isAuth = DBAuthorization.UserAuthorization(login, password, users, ref name);
                    writer.WriteLine(isAuth);

                    
                }
                else if (ans.Equals("Story"))
                {

                    foreach (string line in story.GetStory(someNumber.GetNumber()))
                    {
                        Console.WriteLine($"From {name} -> " + line);
                        writer.WriteLine(line);
                        //writer.Flush();

                        int delay = int.Parse(configTools.GetVariableFromXml("Delay"));

                        await Task.Delay(delay);

                        //Thread.Sleep(int.Parse(Tools.GetVariableFromXml("Delay")));
                    }

                    writer.WriteLine("stop");
                    break;
                }

            }
            reader.Close();
            //writer.Close();
            client.Close();
            Console.WriteLine("Client connection closed!");
        }
        catch (IOException)
        {
            Console.WriteLine("Problem with client communication. Exiting thread.");
        }
        finally
        {
            if (client != null)
            {
                client.Close();
            }
        }
    }

    private static void ShowServerNetworkConfig()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface adapter in adapters)
        {
            Console.WriteLine(adapter.Description);
            Console.WriteLine("\tAdapter Name: " + adapter.Name);
            Console.WriteLine("\tMAC Address: " + adapter.GetPhysicalAddress());
            IPInterfaceProperties ip_properties = adapter.GetIPProperties();
            UnicastIPAddressInformationCollection addresses = ip_properties.UnicastAddresses;
            foreach (UnicastIPAddressInformation address in addresses)
            {
                Console.WriteLine("\tIP Address: " + address.Address);
            }
        }
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void Main()
    {
        
        TcpListener listener = null;
        try
        {
            //ShowServerNetworkConfig();
            listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();
            Console.WriteLine("MultiIPEchoServer started...");
            Console.WriteLine("Waiting for incoming client connections...");
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Accepted new client connection...");
                Thread t = new Thread(ProcessClientRequests);
                t.Start(client);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            if (listener != null)
            {
                listener.Stop();
            }
        }
    }
}
