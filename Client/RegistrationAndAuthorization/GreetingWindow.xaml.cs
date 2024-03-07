using Client.RegistrationAndAuthorization;
using MaterialDesignThemes.Wpf;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Navigation;


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

        public void CloseWindow()
        {
            Client.Close();
            Close();
        }


    }
}