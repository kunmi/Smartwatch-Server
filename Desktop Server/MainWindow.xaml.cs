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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Desktop_Server;
using System.Windows.Forms;
using MahApps.Metro.Controls;

namespace Desktop_Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        const int PORT = 2407;
        bool isServerStarted = false;
        String ipAddress = "";
        ActionController controller;

        VJoyController vjoy;
        SpeechHelper speechMachine;

        public VJoyController VJOY {
            get {
                return vjoy;
            }
        }

        List<Preference.SUPPORTED_MODES> modes = new List<Preference.SUPPORTED_MODES> { 
            Preference.SUPPORTED_MODES.Mouse,
            Preference.SUPPORTED_MODES.Keyboard,
            Preference.SUPPORTED_MODES.Vjoy_Controller
        };
       
        
        

        UdpClient udpClient = new UdpClient(PORT);
        Thread networkThread;
        
       
        public MainWindow()
        {
            InitializeComponent();
            
            vjoy = new VJoyController(this);
            controller = new ActionController(this);
            speechMachine = new SpeechHelper(this);

            if(!vjoy.isVjoyEnabled())
            {
                modes.Remove(Preference.SUPPORTED_MODES.Vjoy_Controller);
            }

            if(System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach(var ip in host.AddressList)
                {
                    if(ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddressLabel.Content = ip.ToString();
                        ipAddress = ip.ToString();
                    }
                }

            }

            modeBox.ItemsSource = modes;
            modeBox.SelectedIndex = 0;

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if(networkThread == null)
            {
                networkThread = new Thread(new ThreadStart(networkStart));
                networkThread.Start();
            }
            
        }

        public void networkStart()
        {
            speechMachine.initalizeEngine();

            isServerStarted = true;
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, PORT);
            string received_data;
            byte[] received_byte_array;
            try
            {

                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    var host = Dns.GetHostEntry(Dns.GetHostName());
                    foreach (var ip in host.AddressList)
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddress = ip.ToString();
                            logmMssage("IP is: " + ipAddress);
                        }
                    }

                }

                while (isServerStarted)
                {
                    //Thread.Sleep(1000);
                    logmMssage("waiting for contact");
                    received_byte_array = udpClient.Receive(ref groupEP);
                    logmMssage("Received MSG from: " + groupEP.ToString());
                    received_data = Encoding.ASCII.GetString(received_byte_array, 0, received_byte_array.Length);
                    logmMssage("Received " + received_data);

                    controller.decodeAction(received_data);
                }
            }
            catch (Exception exp)
            {
                logmMssage(exp.StackTrace);
            }
            udpClient.Close();
        }

        public void logmMssage(String message)
        {
            if (Preference.LOG_ENEBLED)
                System.Threading.ThreadPool.QueueUserWorkItem(
                 (a) =>
                 {
                     System.Threading.Thread.Sleep(500);
                     this.Dispatcher.Invoke((Action)(() =>
                     {
                         logBox.Text = logBox.Text + Environment.NewLine + message;
                     }));
                 }
                 );




           
            
        }

        private void logCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Preference.LOG_ENEBLED = true;
        }

        private void logCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Preference.LOG_ENEBLED = false;
        }

        private void speedMultiPlier_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Preference.MOUSE_SPEED = (int) e.NewValue;
        }

        private void stopServerButton_Click(object sender, RoutedEventArgs e)
        {
            stopServer();
        }

        void stopServer()
        {
            if (networkThread != null)
            {
                networkThread.Abort();
                isServerStarted = false;
                udpClient.Close();
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            vjoy.ReleaseController();
            stopServer();
        }

        private void modeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(modeBox.SelectedItem.Equals(Preference.SUPPORTED_MODES.Vjoy_Controller))
            {
                if(vjoy.isVjoyEnabled())
                vjoy.activateVJoy();
            }
            else
            {
                if (vjoy.isVjoyEnabled())
                vjoy.ReleaseController();
            }

            Preference.ActiveMode = (Preference.SUPPORTED_MODES) modeBox.SelectedItem;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string str =  "{\"type\":\"GYRO\",\"values\":{\"x\":0,\"y\":0.7001794576644897,\"z\":0.25891241431236267}}";
            controller.decodeAction(str);
            //vjoy.OperateJoyStick(int.Parse(testBox.Text),0,0);
        }

        private void invertYBox_Checked(object sender, RoutedEventArgs e)
        {
            Preference.InvertY = true;
        }

        private void _Yunchecked(object sender, RoutedEventArgs e)
        {
            Preference.InvertY = false;
        }


        private void invertAxisBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Preference.InvertAxis = false;
        }


        private void invertAxisBox_Checked(object sender, RoutedEventArgs e)
        {
            Preference.InvertAxis = true;
        }




    
    }
}
