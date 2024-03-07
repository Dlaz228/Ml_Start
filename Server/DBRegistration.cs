using Ml_Start.ConfigurationLibrary;
using Serilog;
using Server.MlStartDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class DBRegistration
    {
        public static void UserRegistration(string login, string password)
        {
            //Console.WriteLine(login);
            //Console.WriteLine(password);
            try
            {
                var context = new MlStartDbContext();

                context.Add(new User { Login = login, Password = Hasher.Hash(password) });

                context.SaveChanges();

                Console.WriteLine($"Пользователь {login} был зарегестрирован");
                LoggingTools.WriteLog("Information", $"Пользователь {login} был зарегестрирован");
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка регистрации");
                LoggingTools.WriteLog("Warning", e.Message);
            }
        }
    }
}
