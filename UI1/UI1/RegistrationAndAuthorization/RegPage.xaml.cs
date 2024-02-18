using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UI1.TestDB;
using Ml_Start.ConfigurationProject;

namespace UI1
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Password;
            string password_2 = textBoxPassword_2.Password;

            if (Validate(login, "login") && Validate(password, "password", password_2))
            {
                var context = new TestBdContext();

                context.Users.Add(new User { Login = login, Password = Tools.Hash(password) });

                context.SaveChanges();

                MessageBox.Show($"Успешно");
            }
        }

        private void Button_Page_Auth_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());
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
