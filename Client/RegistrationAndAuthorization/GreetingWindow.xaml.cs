using Client.RegistrationAndAuthorization;
using Ml_Start.ConfigurationLibrary;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Windows;


namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GreetingWindow : Window
    {
        TcpClient Client;
        ConnectionWindow Window;

        public GreetingWindow(ConnectionWindow window)
        {
            InitializeComponent();
            Window = window;
            Client = window.client;
        }

        private void Button_Window_Reg_Click(object sender, RoutedEventArgs e)
        {
            Greeting.Navigate(new RegPage(Window, this));
            //RegPage regPage = new RegPage(Window);

            //new RegPage(Window).ShowDialog(this);
            //ShowDialog();

            //this.NavigationService.Navigate(new RegPage(Window));
        }

        private void Button_Window_Auth_Click(object sender, RoutedEventArgs e)
        {
            Greeting.Navigate(new AuthPage(Window, this));

            //Greeting.Content = new AuthPage(Window, this);
        }

        //private void Button_Connect_Click(object sender, RoutedEventArgs e)
        //{
        //    //Socket socket = new();
        //    IPAddress ip_address = IPAddress.Parse("127.0.0.1"); //default
        //    int port = 8080;
        //    client = new TcpClient(ip_address.ToString(), port);
        //    //App.Current.Resources["client"] = client;
        //    //Client.client = client;
        //}

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if ((bool)App.Current.Resources["isUserExit"])
            {
                StreamWriter writer = new StreamWriter(Client.GetStream());
                writer.AutoFlush = true;

                MessageBoxResult msgBoxResult = MessageBox.Show("Do you really want to exit?", "Exiting...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (msgBoxResult == MessageBoxResult.Yes)
                {
                    LoggingTools.WriteLog("Information", $"Пользователь закрыл программу");
                    writer.WriteLine("Close");

                    Client.Close();
                    //writer.WriteLine("Close");
                    //Client.Close();
                    //this.Close();
                    //e.Cancel = false;
                    //return;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}