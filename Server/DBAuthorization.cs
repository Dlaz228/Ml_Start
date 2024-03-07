using Server.MlStartDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ml_Start.ConfigurationLibrary;

namespace Server
{
    internal class DBAuthorization
    {
        public static bool UserAuthorization(string login, string password, List<string> users, ref string name)
        {
            try
            {
                var context = new MlStartDbContext();

                var user = context.Users
                           .Where(user => user.Login == login && user.Password == Hasher.Hash(password))
                           .FirstOrDefault();

                if (user != null && users.Contains(login))
                {
                    Console.WriteLine($"Пользователь {login} уже авторизован");
                    return false;
                }
                else if (user != null)
                {
                    name = login;
                    users.Add(name);
                    Console.WriteLine($"Пользователь {login} авторизовался");
                    LoggingTools.WriteLog("Information", $"Пользователь {login} авторизовался");
                    return true;
                }
                else
                {
                    Console.WriteLine("Не удалось найти пользователя");
                    LoggingTools.WriteLog("Information", "Не удалось найти пользователя");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка авторизации");
                LoggingTools.WriteLog("Warning", e.Message);
                return false;
            }
            
        }
    }
}
