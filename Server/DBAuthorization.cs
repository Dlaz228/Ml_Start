using Server.MlStartDB;
using Ml_Start.ConfigurationLibrary;
using static Server.HelpingFunctions;
using System.Net.Sockets;

namespace Server
{
    internal class DBAuthorization
    {
        public static string UserAuthorization(string login, string password, List<string> users, TcpClient client, ref string name)
        {
            try
            {
                var context = new MlStartDbContext();

                var user = context.Users
                           .Where(user => user.Login == login && user.Password == Hasher.Hash(password))
                           .FirstOrDefault();

                if (user != null && users.Contains(login))
                {
                    WriteToConsole($"Пользователь {login}({client.Client.RemoteEndPoint}) уже авторизован");
                    return "taken";
                }
                if (user != null)
                {
                    name = login;
                    //users.Add(name);
                    WriteToConsole($"Пользователь {login}({client.Client.RemoteEndPoint}) авторизовался");
                    LoggingTools.WriteLog("Information", $"Пользователь {login}({client.Client.RemoteEndPoint}) авторизовался");
                    return "true";
                }
                else
                {
                    WriteToConsole($"Не удалось найти пользователя {login}({client.Client.RemoteEndPoint})");
                    LoggingTools.WriteLog("Information", $"Не удалось найти пользователя {login}({client.Client.RemoteEndPoint})");
                    return "false";
                }
            }
            catch (Exception ex)
            {
                WriteToConsole($"Ошибка авторизации клиента({client.Client.RemoteEndPoint})");
                LoggingTools.WriteLog("Warning", $"Ошибка авторизации клиента({client.Client.RemoteEndPoint})", ex);
                return "false";
            }
            
        }
    }
}
