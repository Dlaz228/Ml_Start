using Client.RegistrationAndAuthorization;
using MaterialDesignThemes.Wpf;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        TcpClient Client;
        ConnectionWindow Window;
        GreetingWindow GreetingWindow;

        public AuthPage(ConnectionWindow window, GreetingWindow greetingWindow)
        {
            InitializeComponent();
            Client = window.client;
            Window = window;
            GreetingWindow = greetingWindow;

        }

        private void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = textBoxLogin.Text;
                string password = textBoxPassword.Password;

                StreamReader reader = new StreamReader(Client.GetStream());
                StreamWriter writer = new StreamWriter(Client.GetStream());
                writer.AutoFlush = true;

                writer.WriteLine("Auth");
                writer.WriteLine(login);
                writer.WriteLine(password);

                string isAuth = reader.ReadLine();

                if (isAuth.Equals("True"))
                {
                    MainProgramWindow mainProgramWindow = new MainProgramWindow(Window, GreetingWindow);
                    mainProgramWindow.Show();

                    GreetingWindow.Close();
                }
                else
                {
                    MessageBox.Show($"{isAuth}");
                }
            }
            catch (Exception ex)
            {
                tbConnect.Text = "Соединение потеряно";
                MessageBoxResult res = MessageBox.Show("Вернуться на страницу подключения?");
                if (res == MessageBoxResult.OK)
                {
                    ConnectionWindow connectionWindow = new ConnectionWindow();
                    connectionWindow.Show();

                    GreetingWindow.Close();
                }
            }

            ////var context = new TestBdContext();

            ////var user = context.Users.
            ////           Where(user => user.Login == loginUser && user.Password == Hasher.Hash(passwordUser)).
            ////           FirstOrDefault();

            //if (user != null)
            //{
            //    MessageBox.Show($"Успешно");

            //    //Content = null;



            //    //MainWindow mainWindow = new();
            //    //mainWindow.Close();
            //    //NavigationService.Navigate(null);
            //    //NavigationService.Navigate(null);
            //    MainProgramWindow mainProgramWindow = new();
            //    mainProgramWindow.Show();

            //    Application.Current.MainWindow.Close();


            //    //mainProgram.Show();
            //}
            //else
            //{
            //    MessageBox.Show("Не то...");
            //}
        }

        private void Button_Page_Reg_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new RegPage());
        }

        private void Button_Back_To_Greeting_Click(object sender, RoutedEventArgs e)
        {
            Content = null;
            //NavigationService.NavigateBack();
            //NavigationService.Navigate(new AuthPage());
        }
    }
}
