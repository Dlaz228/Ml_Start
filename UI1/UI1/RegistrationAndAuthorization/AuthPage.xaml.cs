using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UI1.TestDB;
using Ml_Start.ConfigurationLibrary;

namespace UI1
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void Button_Auth_Click(object sender, RoutedEventArgs e)
        {
            string loginUser = textBoxLogin.Text;
            string passwordUser = textBoxPassword.Password;

            var context = new TestBdContext();

            var user = context.Users.
                       Where(user => user.Login == loginUser && user.Password == Hasher.Hash(passwordUser)).
                       FirstOrDefault();

            if (user != null)
            {
                MessageBox.Show($"Успешно");

                //Content = null;



                //MainWindow mainWindow = new();
                //mainWindow.Close();
                //NavigationService.Navigate(null);
                //NavigationService.Navigate(null);
                MainProgramWindow mainProgramWindow = new();
                mainProgramWindow.Show();

                Application.Current.MainWindow.Close();

                
                //mainProgram.Show();
            }
            else
            {
                MessageBox.Show("Не то...");
            }
        }

        private void Button_Page_Reg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegPage());
        }
    }
}
