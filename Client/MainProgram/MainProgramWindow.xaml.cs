using Client.RegistrationAndAuthorization;
using Ml_Start.ConfigurationLibrary;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Windows;
using System.Windows.Interop;


namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainProgram.xaml
    /// </summary>
    /// 

    public partial class MainProgramWindow : Window
    {
        GreetingWindow GreetingWindow;
        TcpClient Client;

        public MainProgramWindow(ConnectionWindow window, GreetingWindow greetingWindow)
        {
            InitializeComponent();
            GreetingWindow = greetingWindow;
            Client = window.client;
            App.Current.Resources["Delay"] = CongfigTools.GetVariableFromXml("Delay");
        }
          
        private void Start_Story_Click(object sender, RoutedEventArgs e)
        {
            int N, L;
            try
            {
                StreamWriter writer = new StreamWriter(Client.GetStream());
                writer.AutoFlush = true;

                CongfigTools.GetVariables(out N, out L);

                writer.WriteLine("Story");
                writer.WriteLine(N);
                writer.WriteLine(L);

                Thread t = new Thread(GetStory);
                t.Start();
            }
            catch (Exception ex)
            {
                Disconnect(ex);
            }
            
        }

        public void GetStory()
        {
            

            StreamReader reader = new StreamReader(Client.GetStream());
            StreamWriter writer = new StreamWriter(Client.GetStream());
            writer.AutoFlush = true;

            string line = "";
            
            //btStartStory.IsEnabled = false;
            EnableAndDisableButton(true);

            while (true)
            {
                try
                {
                    line = reader.ReadLine();

                    if (line.Equals("stop"))
                    {
                        //btStartStory.IsEnabled = true;
                        EnableAndDisableButton(false);
                        break;
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        storyText.Text = line;
                    });

                    writer.WriteLine(App.Current.Resources["Delay"]);
                }
                catch (Exception ex)
                {
                    Disconnect(ex);
                    break;
                }
            }

            //reader.Close();
            //writer.Close();
        }

        private void Disconnect(Exception ex)
        {
            this.Dispatcher.Invoke(() =>
            {
                Client.Close();
                tbConnect.Text = "Соединение потеряно";
                MessageBoxResult res = MessageBox.Show("Произошла ошибка!\nВернуться на страницу подключения?", "", MessageBoxButton.OKCancel);

                if (res == MessageBoxResult.Cancel)
                {
                    this.Close();
                }
                else if (res == MessageBoxResult.OK)
                {
                    ConnectionWindow connectionWindow = new ConnectionWindow();
                    connectionWindow.Show();

                    this.Close();
                }
            });
        }

        public void Image_Page_Options_Click(object sender, RoutedEventArgs e)
        {
            OptionsWindow optionsWindow = new();
            optionsWindow.Show();
        }

        private void EnableAndDisableButton(bool isEnable)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (isEnable)
                {
                    btStartStory.IsEnabled = false;
                }
                else
                {
                    btStartStory.IsEnabled = true;
                }
            });

            
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            StreamWriter writer = new StreamWriter(Client.GetStream());
            writer.AutoFlush = true;

            MessageBoxResult msgBoxResult = MessageBox.Show("Do you really want to exit?", "Exiting...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (msgBoxResult == MessageBoxResult.Yes)
            {
                
                writer.WriteLine("Close");
                Client.Close();
                //Close();
                e.Cancel = false;
                //return;
            }
            else
            {
                e.Cancel = true;
            }

            //MessageBoxResult res = MessageBox.Show("Точно выходишь?", "", MessageBoxButton.OKCancel);
            //if (res == MessageBoxResult.OK)
            //{
            //    writer.WriteLine("close");
            //    Client.Close();
            //    Close();
            //}

            
        }
    }
}
