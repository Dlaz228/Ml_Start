using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TransferObjectLibrary
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "ITransferObject" в коде и файле конфигурации.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class TransferObject : ITransferObject
    {
        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;

        public int Connect(string name)
        {
            ServerUser user = new ServerUser()
            {
                ID = nextId++,
                Name = name,
                operationContext = OperationContext.Current
            };

            SendMsg(user.Name + "поключился к чату", 0);

            users.Add(user);

            return user.ID;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(x => x.ID == id);

            if (user != null)
            {
                users.Remove(user);
                SendMsg(user.Name + "отключился от чата", 0);
            }
        }

        public void SendMsg(string msg, int id)
        {
            foreach (var item in users)
            {
                string answer = DateTime.Now.ToShortTimeString();

                var user = users.FirstOrDefault(x => x.ID == id);

                if (user != null)
                {
                    answer += ": " + user.Name + " ";
                }

                answer += msg;

                item.operationContext.GetCallbackChannel<IServerCallback>().MsgCallback(answer);
            }
        }

        public int GetSum(int a, int b)
        {
            return a + b;
        }
    }
}
