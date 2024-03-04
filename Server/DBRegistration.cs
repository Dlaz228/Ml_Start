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

            var context = new MlStartDbContext();

            context.Add(new User { Login = login, Password = Hasher.Hash(password) });

            context.SaveChanges();

            Console.WriteLine($"The {login} has been registered");
        }

        
    }
}
