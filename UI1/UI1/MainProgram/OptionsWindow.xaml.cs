using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.IdentityModel.Tokens;
using Ml_Start.ConfigurationProject;

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
            //ConfigurationMethods configurationMethods = new();

            //configurationMethods.ChangeDelay(textBoxDelay.Text);

            Tools.ChangeDelay(textBoxDelay.Text);

            MessageBox.Show("Успешно!");

            Close();
        }

        
    }
}
