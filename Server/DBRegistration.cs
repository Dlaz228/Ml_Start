using Ml_Start.ConfigurationLibrary;
using Server.MlStartDB;
using System.Net.Sockets;
using static Server.HelpingFunctions;


namespace Server
{
    internal class DBRegistration
    {
        public static void UserRegistration(string login, string password, TcpClient client)
        {
            //Console.WriteLine(login);
            //Console.WriteLine(password);
            try
            {
                var context = new MlStartDbContext();

                context.Add(new User { Login = login, Password = Hasher.Hash(password) });

                context.SaveChanges();

                WriteToConsole($"Пользователь {login}({client.Client.RemoteEndPoint}) был зарегестрирован");
                LoggingTools.WriteLog("Information", $"Пользователь {login}({client.Client.RemoteEndPoint}) был зарегестрирован");
            }
            catch (Exception ex)
            {
                WriteToConsole($"Ошибка регистрации клиента(({client.Client.RemoteEndPoint}))");
                LoggingTools.WriteLog("Warning", $"Ошибка регистрации клиента(({client.Client.RemoteEndPoint}))", ex);
            }
        }
    }
}
