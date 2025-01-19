using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace MSFSClient
{

    public partial class FormClient : Form
    {
        private IniFile iniFile;
        private bool isConnected = false;
        private TcpClient client;
        private NetworkStream stream;
        private Thread sendThread;
        private ConcurrentQueue<string> commandQueue = new ConcurrentQueue<string>();


        private bool sendTaxiLightCommand = false;

        public FormClient()
        {
            InitializeComponent();
            iniFile = new IniFile("Settings.ini");
            textBoxIP.Text = iniFile.Read("Settings", "IPAddress");
            textBoxPort.Text = iniFile.Read("Settings", "Port");
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProxy_FormClosing);

            pict_Light_Landing.Click += new EventHandler(pict_Light_Landing_Click);
            pict_Lights_Taxi.Click += new EventHandler(pict_Lights_Taxi_Click);
            pict_Light_Beacon.Click += new EventHandler(pict_Light_Beacon_Click);
            but_Lights_Taxi.Click += new EventHandler(but_Lights_Taxi_Click);
        }

        private void pict_Light_Landing_Click(object sender, EventArgs e)
        { // Your click event logic here
          Console.WriteLine("pict_Landing clicked!");
        }

        private void pict_Lights_Taxi_Click(object sender, EventArgs e)
        { // Your click event logic here
          Console.WriteLine("pict_Taxi clicked!");
        }

        private void pict_Light_Beacon_Click(object sender, EventArgs e)
        { // Your click event logic here
          Console.WriteLine("pict_Beacon clicked!");
        }

        private void but_Lights_Taxi_Click(object sender, EventArgs e)
        {
            commandQueue.Enqueue("LIGHT_TAXI_ON");
            Console.WriteLine("LIGHT_TAXI");
        }

        private void button_Connect_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                Connect();
            }
            else
            {
                Disconnect();
            }
        }

        private void Connect()
        {
            string ipAddress = textBoxIP.Text;
            int port;

            if (!int.TryParse(textBoxPort.Text, out port))
            {
                MessageBox.Show("Please enter a valid port number.");
                return;
            }
            
            try
            {
                client = new TcpClient(ipAddress, port);
                stream = client.GetStream();

                button_Connect.Text = "Disconnect";
                isConnected = true;

                // Start the sending thread
                Thread sendDataThread = new Thread(SendData);
                sendDataThread.IsBackground = true;
                sendDataThread.Start();

                // Start the receiving thread
                Thread receiveThread = new Thread(ReceiveData);
                receiveThread.IsBackground = true;
                receiveThread.Start();

                txtResults.Text = "Connected!";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Disconnect()
        {
            try
            {
                if (stream != null)
                    stream.Close();

                if (client != null)
                    client.Close();

                // Change the button text back to "Connect"
                button_Connect.Text = "Connect";
                isConnected = false;
                txtResults.Text = "Disconnected";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void SendData()
        {
            while (isConnected)
            {
                string message = "Client sent: " + DateTime.Now.ToString("HH:mm:ss");

                // Process commands from the queue
                while (commandQueue.TryDequeue(out string command))
                {
                    message += " " + command;
                }

                //if (sendTaxiLightCommand)
                //{
                //    message += " LIGHT_TAXI_ON";
                //    sendTaxiLightCommand = false; // Reset the flag after sending the command
                //}

                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Thread.Sleep(100);
            }
        }

        private void ReceiveData()
        {
            while (isConnected)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // Update the UI with the received data
                    Invoke(new Action(() =>
                    {
                        txtResults.Text = "Received: " + receivedData;
                    }));
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() =>
                    {
                        txtResults.Text = "Error: " + ex.Message;
                    }));
                    break;
                }
            }
        }

        private void FormProxy_FormClosing(object sender, FormClosingEventArgs e)
        {
            iniFile.Write("Settings", "IPAddress", textBoxIP.Text);
            iniFile.Write("Settings", "Port", textBoxPort.Text);
            Console.WriteLine("Settings.ini updated");
        }

    }

}
