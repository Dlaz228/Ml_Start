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
        //ConnectionWindow Window;

        public GreetingWindow()
        {
            InitializeComponent();
        }

        private void Button_Window_Reg_Click(object sender, RoutedEventArgs e)
        {
            Greeting.Navigate(new RegPage(this));
            //RegPage regPage = new RegPage(Window);

            //new RegPage(Window).ShowDialog(this);
            //ShowDialog();

            //this.NavigationService.Navigate(new RegPage(Window));
        }

        private void Button_Window_Auth_Click(object sender, RoutedEventArgs e)
        {
            Greeting.Navigate(new AuthPage(this));

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
                MessageBoxResult msgBoxResult = MessageBox.Show("Do you really want to exit?", "Exiting...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (msgBoxResult == MessageBoxResult.Yes)
                {
                    Client.Close();
                    LoggingTools.WriteLog("Information", $"Пользователь закрыл программу");

                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}