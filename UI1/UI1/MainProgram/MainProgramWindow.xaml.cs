using System;
using System.Collections.Generic;
using System.Linq;
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
using Ml_Start.MakeStory;
using Ml_Start.GenerateSomeNumber;
using Ml_Start.ConfigurationProject;
using System.Windows.Navigation;


namespace UI1
{
    /// <summary>
    /// Логика взаимодействия для MainProgram.xaml
    /// </summary>
    /// 

    public partial class MainProgramWindow : Window
    {
        CancellationTokenSource cts = new();

        public MainProgramWindow()
        {
            InitializeComponent();
        }
          
        private async void Start_Story_Click(object sender, RoutedEventArgs e)
        {
            Story story = new();
            NumberCreator someNumber = new();
            Tools tools = new();

            while (true)
            {
                foreach (string line in story.GetStory(someNumber.GetNumber()))
                {
                    if (cts.IsCancellationRequested)
                        return;

                    storyText.Text = line;

                    int delay = int.Parse(tools.GetVariableFromXml("Delay"));

                    await Task.Delay(delay);

                    //Thread.Sleep(int.Parse(Tools.GetVariableFromXml("Delay")));
                }

                storyText.Text = "Где-то в другом мире..";

                await Task.Delay(10000);
            }
        }

        public void Image_Page_Options_Click(object sender, RoutedEventArgs e)
        {
            OptionsWindow optionsWindow = new();
            optionsWindow.Show();
        }

        public void Stop_Story_Click(object sender, RoutedEventArgs e)
        {
            if (cts != null)
                cts.Cancel();
        }
    }
}
