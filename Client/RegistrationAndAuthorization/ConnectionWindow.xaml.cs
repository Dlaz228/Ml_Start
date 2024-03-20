using System.Net.Sockets;
using System.Net;
using System.Windows;
using Ml_Start.ConfigurationLibrary;

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
            LoggingTools.CreateLogger();
            CongfigTools.CreateClientConfigXmlFile();
            App.Current.Resources["isUserExit"] = true;
        }

        private void Button_Connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validate(textBoxIp.Text, textBoxPort.Text))
                {
                    IPAddress ip_address = IPAddress.Parse(textBoxIp.Text); //default
                    int port = int.Parse(textBoxPort.Text);
                    client = new TcpClient(ip_address.ToString(), port);
                    LoggingTools.WriteLog("Debug", "Новый клиент присоединился к серверу");
                    App.Current.Resources["Client"] = client;
                    GreetingWindow greetingWindow = new GreetingWindow();
                    greetingWindow.Show();
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при присоединении к серверу"); 
                LoggingTools.WriteLog("Error", "Произошла ошибка при присоединении к серверу", ex);
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
