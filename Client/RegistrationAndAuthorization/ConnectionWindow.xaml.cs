using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;
using MaterialDesignThemes.Wpf;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Client.RegistrationAndAuthorization
{
    /// <summary>
    /// Логика взаимодействия для ConnectionWindow.xaml
    /// </summary>
    public partial class ConnectionWindow : Window
    {
        public TcpClient client;

        public ConnectionWindow()
        {
            InitializeComponent();
        }

        private void Button_Connect_Click(object sender, RoutedEventArgs e)
        {
            //Socket socket = new();
            if (Validate(textBoxIp.Text, textBoxPort.Text))
            {
                IPAddress ip_address = IPAddress.Parse(textBoxIp.Text); //default
                int port = int.Parse(textBoxPort.Text);
                client = new TcpClient(ip_address.ToString(), port);

                GreetingWindow greetingWindow = new GreetingWindow(this);
                greetingWindow.Show();
                Close();
            }
            //App.Current.Resources["client"] = client;
            //Client.client = client;
        }

        private bool Validate(string address, string number)
        {
            int port;
            IPAddress ipAdress;

            if (address.Length < 1)
            {
                MessageBox.Show("Address is required");
                return false;
            }
            else
            {
                try
                {
                    ipAdress = IPAddress.Parse(address);
                }
                catch
                {
                    MessageBox.Show("Address is not valid");
                    return false;
                }
            }

            if (number.Length < 1)
            {
                MessageBox.Show("Port number is required");
                return false;
            }
            else if (!int.TryParse(number, out port))
            {
                MessageBox.Show("Port number is not valid");
                return false;
            }
            else if (port < 0 || port > 65535)
            {
                MessageBox.Show("Port number is out of range");
                return false;
            }
            
            return true;
        }
    }
}
