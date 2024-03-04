using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.IO;
using System.Net.Sockets;
using Client.RegistrationAndAuthorization;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        TcpClient Client;
        GreetingWindow GreetingWindow;

        public RegPage(ConnectionWindow window, GreetingWindow greetingWindow)
        {
            InitializeComponent();
            Client = window.client;
            GreetingWindow = greetingWindow;
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            //var client = Client.client;

            string login = textBoxLogin.Text;
            string password = textBoxPassword.Password;
            string password_2 = textBoxPassword_2.Password;

            if (Validate(login, "login") || Validate(password, "password", password_2))
            {
                try
                {
                    StreamWriter writer = new StreamWriter(Client.GetStream());
                    writer.AutoFlush = true;
                    writer.WriteLine("Reg");
                    writer.WriteLine(login);
                    writer.WriteLine(password);

                    MessageBox.Show($"Успешно");
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
            }
        }

        private void Button_Back_To_Greeting_Click(object sender, RoutedEventArgs e)
        {
            Content = null;
            //NavigationService.NavigateBack();
            //NavigationService.Navigate(new AuthPage());
        }

        private bool Validate(string variableValue, string variable, string? password2 = null)
        {
            switch (variable)
            {
                case "login":
                    if (String.IsNullOrWhiteSpace(variable))
                    {
                        MessageBox.Show("Поле логин не может быть пустым!");
                        return false;
                    }
                    else if (variableValue.Length < 5)
                    {
                        MessageBox.Show("Длина не может быть меньше 5 символов");
                        return false;
                    }
                    else if (variableValue.Length > 50)
                    {
                        MessageBox.Show("Длина не может быть больше 50 символов");
                        return false;
                    }
                    else if (!variableValue.Any(Char.IsLetter))
                    {
                        MessageBox.Show("Необходима хотя бы одна буква");
                        return false;
                    }
                    else if (!variableValue.Any(Char.IsUpper))
                    {
                        MessageBox.Show("Необходима хотя бы одна заглавная буква");
                        return false;
                    }
                    else if (!variableValue.Any(Char.IsLower))
                    {
                        MessageBox.Show("Необходима хотя бы одна строчная буква");
                        return false;
                    }
                    return true;

                case "password":
                    if (String.IsNullOrWhiteSpace(variable))
                    {
                        MessageBox.Show("Поле пароля не может быть пустым!");
                        return false;
                    }
                    else if (variableValue.Length < 5)
                    {
                        MessageBox.Show("Длина не может быть меньше 5 символов");
                        return false;
                    }
                    else if (variableValue.Length > 50)
                    {
                        MessageBox.Show("Длина не может быть больше 50 символов");
                        return false;
                    }
                    else if (variable != password2)
                    {
                        MessageBox.Show("Пароль и повторный пароль не совпадают");
                        return false;
                    }
                    return true;
            }

            return true;
        }
    }
}
