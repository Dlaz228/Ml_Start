using Ml_Start.ConfigurationLibrary;
using System.Text.RegularExpressions;
using System.Windows;

namespace UI1
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
            if (Regex.IsMatch(textBoxDelay.Text, @"^\d+$"))
            {
                CongfigTools.ChangeDelay(textBoxDelay.Text);

                MessageBox.Show("Успешно!");

                Close();
            }
            
            else
            {
                textBoxDelay.Text = "";
                MessageBox.Show("Ты ввёл не число!");
            }
            
        }

        
    }
}
