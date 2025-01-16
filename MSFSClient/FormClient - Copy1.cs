using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;

namespace MSFSClient
{

    public partial class FormClient : Form
    {
        private bool isConnected = false;
        private TcpClient client;
        private NetworkStream stream;

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

                //// Optionally send an initial message
                //string dataToSend = "Hello, Server!";
                //byte[] data = Encoding.UTF8.GetBytes(dataToSend);
                //stream.Write(data, 0, data.Length);

                //// Receive response (for demonstration)
                //byte[] buffer = new byte[1024];
                //int bytesRead = stream.Read(buffer, 0, buffer.Length);
                //string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                //txtResults.Text = "Received: " + receivedData;

                // Create an object to send
                var dataObject = new { Message = "Hello, Server!", Value = 42 };

                // Serialize data
                string dataToSend = JsonConvert.SerializeObject(dataObject);
                byte[] data = Encoding.UTF8.GetBytes(dataToSend);
                stream.Write(data, 0, data.Length);

                // Optionally receive response (for demonstration)
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                txtResults.Text = "Received: " + receivedData;
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
    }
}
