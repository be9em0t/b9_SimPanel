using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace MSFSClient
{

    public partial class FormClient : Form
    {
        private bool isConnected = false;
        private TcpClient client;
        private NetworkStream stream;
        private Thread sendThread;

        public FormClient()
        {
            InitializeComponent();
            textBoxIP.Text = "127.0.0.1";
            textBoxPort.Text = "505";
        }

        private void button_Connect_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                // Connect
                Connect();
            }
            else
            {
                // Disconnect
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

                // Change the button text to "Disconnect"
                button_Connect.Text = "Disconnect";
                isConnected = true;

                // Start the sending thread
                sendThread = new Thread(SendData);
                sendThread.IsBackground = true;
                sendThread.Start();

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

                if (sendThread != null && sendThread.IsAlive)
                    sendThread.Abort();
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
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Thread.Sleep(1000);
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

    }
}
