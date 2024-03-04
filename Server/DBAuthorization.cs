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
            var context = new MlStartDbContext();

            var user = context.Users
                       .Where(user => user.Login == login && user.Password == Hasher.Hash(password))
                       .FirstOrDefault();

            if (user != null)
            {
                Console.WriteLine($"Пользователь {login} авторизовался");
                name = login;
                users.Add(name);
                return true;
            }
            else
            {
                Console.WriteLine("Ошибка авторизации");
                return false;
            }
        }
    }
}
