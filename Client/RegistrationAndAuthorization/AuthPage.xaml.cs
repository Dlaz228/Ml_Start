using Client.RegistrationAndAuthorization;
using Ml_Start.ConfigurationLibrary;
using System.IO;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

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

            //tbClientIP.Text = Client.Client.LocalEndPoint.ToString();
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

                if (isAuth.Equals("true"))
                {
                    LoggingTools.WriteLog("Information", $"{login} авторизовался");
                    MainProgramWindow mainProgramWindow = new MainProgramWindow(Window, GreetingWindow);
                    mainProgramWindow.Show();

                    App.Current.Resources["isUserExit"] = false;
                    GreetingWindow.Close();
                    App.Current.Resources["isUserExit"] = true;
                }
                else if (isAuth.Equals("taken"))
                {
                    MessageBox.Show($"Пользователь под таким именем уже авторизован");
                    LoggingTools.WriteLog("Debug", $"Пользователь {login} уже авторизован");
                }
                else
                {
                    MessageBox.Show($"Пользователь не найден.\nПопробуйте зарегестрироваться");
                    LoggingTools.WriteLog("Debug", $"Пользователь {login} не найден");
                }
            }
            catch (Exception ex)
            {
                tbConnect.Text = "Соединение потеряно";
                MessageBoxResult res = MessageBox.Show("Произошла ошибка!\nВернуться на страницу подключения?");
                LoggingTools.WriteLog("Error", "Произошла ошибка при авторизации", ex);
                if (res == MessageBoxResult.OK)
                {
                    ConnectionWindow connectionWindow = new ConnectionWindow();
                    connectionWindow.Show();
                    App.Current.Resources["isUserExit"] = false;
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
