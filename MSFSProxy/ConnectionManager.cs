using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MSFServer
{
    public class ConnectionManager
    {
        public bool IsListening { get; private set; }
        public bool IsSimAllowed { get; set; }
        public bool IsSimRawData { get; set; }

        private TcpListener server;
        private TcpClient client;
        private NetworkStream stream;
        private Thread listenThread;
        private SimConnection simConnection;
        private IntPtr windowHandle;
        //// Define the ClientDataReceived event
        //public event Action<string> ClientDataReceived;

        public ConnectionManager(IntPtr handle, int port)
        {
            server = new TcpListener(IPAddress.Any, port);
            simConnection = new SimConnection();
            windowHandle = handle;
            // Subscribe to the SimDataReceived event
            DataQueues.SimDataReceived += OnSimDataReceived;
        }

        public void Start()
        {
            server.Start();
            IsListening = true;
            if (IsSimAllowed == true)
            {
                simConnection?.Initialize(windowHandle); // Pass the window handle
                simConnection?.Start(IsSimRawData);
            }

            listenThread = new Thread(ListenForClients);
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        public void Stop()
        {
            IsListening = false;
            simConnection?.Stop();
            client?.Close();
            server?.Stop();
        }

        private void ListenForClients()
        {
            try
            {
                while (IsListening)
                {
                    client = server.AcceptTcpClient();
                    stream = client?.GetStream();

                    Thread receiveThread = new Thread(OnClientDataReceived);
                    receiveThread.IsBackground = true;
                    receiveThread.Start();
                }
            }
            catch (SocketException ex)
            {
                if (IsListening)
                {
                    MessageBox.Show("Error in ListenForClients: " + ex.Message);
                }
                else
                {
                    Console.WriteLine("Bye!");
                }
            }
        }

        private void OnSimDataReceived(string data)
        {
            Console.WriteLine("> Queue " + data);

            if (client?.Connected == true)
            {
                try
                {
                    byte[] bytesToSend = Encoding.UTF8.GetBytes(data);
                    stream?.Write(bytesToSend, 0, bytesToSend.Length);
                    Console.WriteLine("> Client" + data);

                    // Clear the queue
                    while (DataQueues.SimReceiveQueue.TryDequeue(out _)) { }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in OnSimDataReceived: " + ex.Message);
                }
            }
        }

        //private void OnClientDataReceived()
        //{
        //    while (IsListening && client.Connected)
        //    {
        //        try
        //        {
        //            byte[] buffer = new byte[1024];
        //            int bytesRead = stream.Read(buffer, 0, buffer.Length);
        //            if (bytesRead == 0) break;  // Connection closed

        //            // Message structure
        //            string receivedSimData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        //            Console.WriteLine("< Client" + receivedSimData);

        //            // Raise the event
        //            ClientDataReceived?.Invoke(receivedSimData);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Error in OnClientDataReceived: " + ex.Message);
        //            break;
        //        }
        //    }
        //    // Clean-up after client disconnects
        //    stream?.Close();
        //    client?.Close();
        //}

        private void OnClientDataReceived()
        {
            while (IsListening && client?.Connected == true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;  // Connection closed

                    // Message structure
                    string receivedSimData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("< Client" + receivedSimData);

                    // Check for specific commands to send to SimConnect
                    if (receivedSimData.Contains("LIGHT_TAXI_ON"))
                    {
                        simConnection?.SendTaxiLightEvent();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in OnClientDataReceived: " + ex.Message);
                    break;
                }
            }
            // Clean-up after client disconnects
            stream?.Close();
            client?.Close();
        }

        public void ReceiveMessage(Message m)
        {
            if (simConnection != null)
            {
                simConnection?.ReceiveMessage(m);
            }
        }

        public static string GetLocalIPv4()
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
