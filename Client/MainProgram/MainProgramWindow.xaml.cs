using Client.RegistrationAndAuthorization;
using Ml_Start.ConfigurationLibrary;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Windows;


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
        Thread t;
        //bool isExit;

        public MainProgramWindow(ConnectionWindow window, GreetingWindow greetingWindow)
        {
            InitializeComponent();
            GreetingWindow = greetingWindow;
            Client = window.client;

            App.Current.Resources["Delay"] = CongfigTools.GetVariableFromXml("Delay");
            App.Current.Resources["DelayForNextDay"] = CongfigTools.GetVariableFromXml("DelayForNextDay");
            App.Current.Resources["N"] = CongfigTools.GetVariableFromXml("FirstName");
            App.Current.Resources["L"] = CongfigTools.GetVariableFromXml("LastName");

            //tbClientIP.Text = Client.Client.RemoteEndPoint.ToString();
        }
          
        private void Start_Story_Click(object sender, RoutedEventArgs e)
        {
            //int N, L;
            try
            {
                StreamWriter writer = new StreamWriter(Client.GetStream());
                writer.AutoFlush = true;

                //CongfigTools.GetVariables(out N, out L);

                writer.WriteLine("Story");
                //writer.WriteLine(N);
                //writer.WriteLine(L);

                t = new Thread(GetStory);
                t.Start();
            }
            catch (Exception ex)
            {
                LoggingTools.WriteLog("Error", $"Произошла ошибка при получении истории", ex);
                App.Current.Resources["isUserExit"] = false;
                Disconnect();
            }
        }

        public void GetStory()
        {
            bool isDayOver = true;

            try
            {
                StreamReader reader = new StreamReader(Client.GetStream());
                StreamWriter writer = new StreamWriter(Client.GetStream());
                writer.AutoFlush = true;

                //CancellationToken ct;

                string line = "";

                //btStartStory.IsEnabled = false;
                EnableAndDisableButton(true);

                while (true)
                {
                    writer.WriteLine(App.Current.Resources["N"]);
                    writer.WriteLine(App.Current.Resources["L"]);

                    isDayOver = true;

                    while (isDayOver)
                    {
                        line = reader.ReadLine();

                        isDayOver = ChangeStoryLine(line, writer);
                        //Console.WriteLine(isDayOver);

                        if (isDayOver == false)
                        {
                            break;
                        }

                        writer.WriteLine(App.Current.Resources["Delay"]);
                    }

                    //Console.WriteLine();
                }
                
                //Console.WriteLine();
            }
            catch (Exception ex)
            {
                LoggingTools.WriteLog("Error", $"Произошла ошибка при получении истории", ex);
                App.Current.Resources["isUserExit"] = false;
                Disconnect();
            }

            //reader.Close();
            //writer.Close();
        }

        private bool ChangeStoryLine(string line, StreamWriter writer)
        {
            return this.Dispatcher.Invoke(bool() =>
            {
                if (line.Equals("stop"))
                {
                    storyText.Text = "Начинается новый день...";
                    writer.WriteLine(App.Current.Resources["DelayForNextDay"]);
                    //writer.Flush();
                    return false;
                }
                else
                {
                    storyText.Text = line;
                    return true;
                }
            });
        }

        private void Disconnect()
        {
            //t.Abort();
            if ((bool)App.Current.Resources["isUserExit"])
            {
                Client.Close();
                LoggingTools.WriteLog("Information", $"Пользователь закрыл программу");
                
                this.Close();
            }
            else
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
                        App.Current.Resources["isUserExit"] = false;
                        this.Close();
                    }
                });
            }
        }

        //private static void Disconnect(TcpClient Client)
        //{
        //    StreamWriter writer = new StreamWriter(Client.GetStream());
        //    writer.AutoFlush = true;
        //    writer.WriteLine("Close");
        //    Client.Close();
        //}

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
            if ((bool)App.Current.Resources["isUserExit"])
            {
                StreamWriter writer = new StreamWriter(Client.GetStream());
                writer.AutoFlush = true;

                MessageBoxResult msgBoxResult = MessageBox.Show("Do you really want to exit?", "Exiting...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (msgBoxResult == MessageBoxResult.Yes)
                {
                    //isExit = true;
                    Disconnect();
                    //writer.WriteLine("Close");
                    //Client.Close();
                    //this.Close();
                    //e.Cancel = false;
                    //return;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
