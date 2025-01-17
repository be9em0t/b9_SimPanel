using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using System.Threading;

namespace MSFServer
{
    public partial class FormProxy : Form
    {
        private TcpListener server;
        private TcpClient client;
        private NetworkStream stream;
        private Thread listenThread;
        private bool isListening = false;

        public FormProxy()
        {
            InitializeComponent();
            textBoxIP.Text = GetLocalIPv4();
            textBoxPort.Text = "505";
            button_Serve.Click += new EventHandler(button_Serve_Click);
        }

        private void button_Serve_Click(object sender, EventArgs e)
        {
            if (!isListening)
            {
                // Start the server
                Serve();
            }
            else
            {
                // Stop the server
                StopServer();
            }
        }

        private void Serve()
        {
            int port;
            if (!int.TryParse(textBoxPort.Text, out port))
            {
                MessageBox.Show("Please enter a valid port number.");
                return;
            }

            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            isListening = true;
            button_Serve.Text = "Stop";

            listenThread = new Thread(ListenForClients);
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void StopServer()
        {
            try
            {
                isListening = false;

                if (client != null)
                    client.Close();
                if (server != null)
                    server.Stop();

                button_Serve.Text = "Serve";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ListenForClients()
        {
            try
            {
                while (isListening)
                {
                    client = server.AcceptTcpClient();
                    stream = client.GetStream();

                    Thread receiveThread = new Thread(HandleClientComm);
                    receiveThread.IsBackground = true;
                    receiveThread.Start();
                }
            }
            catch (SocketException ex)
            {
                if (isListening)
                {
                    Invoke(new Action(() => MessageBox.Show("Error: " + ex.Message)));
                }
                else
                {
                    // Server stopped, ignore the exception
                }
            }
        }

        private void HandleClientComm()
        {
            while (isListening && client.Connected)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;  // Connection closed

                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received: " + receivedData);

                    string response = "Proxy sent: " + receivedData;
                    byte[] data = Encoding.UTF8.GetBytes(response);
                    stream.Write(data, 0, data.Length);
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() => MessageBox.Show("Error: " + ex.Message)));
                    break;
                }
            }

            // Clean-up after client disconnects
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
        }


        static string GetLocalIPv4()
        {
            string localIP = string.Empty;
            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            Console.WriteLine(localIP);
            return localIP;
        }

    }
}
