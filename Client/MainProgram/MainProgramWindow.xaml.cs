using Client.RegistrationAndAuthorization;
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
        }
          
        private void Start_Story_Click(object sender, RoutedEventArgs e)
        {
            
            StreamWriter writer = new StreamWriter(Client.GetStream());

            writer.WriteLine("Story");
            writer.Flush();

            Thread t = new Thread(GetStory);
            t.Start();
        }

        public void GetStory()
        {
            StreamReader reader = new StreamReader(Client.GetStream());

            string line = "";

            while (true)
            {
                try
                {
                    line = reader.ReadLine();

                    if (line.Equals("stop"))
                    {
                        break;
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        storyText.Text = line;
                    });
                }
                catch
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        tbConnect.Text = "Соединение потеряно";
                        MessageBoxResult res = MessageBox.Show("Вернуться на страницу подключения?", "", MessageBoxButton.OKCancel);

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

                    break;
                }

            }
        }
    }
}
