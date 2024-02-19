using Ml_Start.ConfigurationLibrary;
using Ml_Start.GenerateSomeNumber;
using Ml_Start.MakeStory;
using System.Windows;


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
            CongfigTools configTools = new();

            while (true)
            {
                foreach (string line in story.GetStory(someNumber.GetNumber()))
                {
                    if (cts.IsCancellationRequested)
                        return;

                    storyText.Text = line;

                    int delay = int.Parse(configTools.GetVariableFromXml("Delay"));

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
