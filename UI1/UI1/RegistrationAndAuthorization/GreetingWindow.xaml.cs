using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GreetingWindow : Window
    {
        public GreetingWindow()
        {
            InitializeComponent();
        }

        private void Button_Window_Reg_Click(object sender, RoutedEventArgs e)
        {
            Greeting.Content = new RegPage();
        }

        private void Button_Window_Auth_Click(object sender, RoutedEventArgs e)
        {
            Greeting.Content = new AuthPage();
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}