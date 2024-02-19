using System.Windows;


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