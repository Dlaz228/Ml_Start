using System.Text.RegularExpressions;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для Options.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public OptionsWindow()
        {
            InitializeComponent();
            
        }

        public void Change_Delay_Click(object sender, RoutedEventArgs e)
        {
            if (Regex.IsMatch(tbDelay.Text, @"^\d+$"))
            {
                App.Current.Resources["Delay"] = tbDelay.Text;

                MessageBox.Show("Успешно!");

                Close();
            }

            else
            {
                tbDelay.Text = "";
                MessageBox.Show("Ты ввёл не число!");
            }

        }

        public void Change_DelayForNextDay_Click(object sender, RoutedEventArgs e)
        {
            if (Regex.IsMatch(tbDelayForNextDay.Text, @"^\d+$"))
            {
                App.Current.Resources["DelayForNextDay"] = tbDelayForNextDay.Text;

                MessageBox.Show("Успешно!");

                Close();
            }
            else
            {
                tbDelayForNextDay.Text = "";
                MessageBox.Show("Ты ввёл не число!");
            }
        }

        public void Change_NValue_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Resources["N"] = tbNValue.Text;

            MessageBox.Show("Успешно!\nИзменения вступят в силу со следующего дня");

            Close();
        }

        public void Change_LValue_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Resources["L"] = tbLValue.Text;

            MessageBox.Show("Успешно!\nИзменения вступят в силу со следующего дня");

            Close();
        }
    }
}
