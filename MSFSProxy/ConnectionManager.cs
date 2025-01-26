﻿using System;
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


        public ConnectionManager(IntPtr handle, int port)
        {
            server = new TcpListener(IPAddress.Any, port);
            simConnection = new SimConnection();
            windowHandle = handle;
            // Subscribe to the DataReceived event
            DataQueues.DataReceived += OnDataReceived;
        }

        public void Start()
        {
            server.Start();
            IsListening = true;
            if (IsSimAllowed == true)
            {
                simConnection.Initialize(windowHandle); // Pass the window handle
                simConnection.Start(IsSimRawData);
            }

            listenThread = new Thread(ListenForClients);
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        public void Stop()
        {
            IsListening = false;
            if (simConnection != null)
            {
                simConnection.Stop();
            }

            if (client != null)
                client.Close();
            if (server != null)
                server.Stop();
        }

        private void ListenForClients()
        {
            try
            {
                while (IsListening)
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

        private void OnDataReceived(string data)
        {
            Console.WriteLine("> Queue " + data);

            if (client != null && client.Connected)
            {
                try
                {
                    byte[] bytesToSend = Encoding.UTF8.GetBytes(data);
                    stream.Write(bytesToSend, 0, bytesToSend.Length);
                    Console.WriteLine("> Client" + data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in OnDataReceived: " + ex.Message);
                }
            }
        }

        private void HandleClientComm()
        {
            while (IsListening && client.Connected)
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
                        simConnection.SendTaxiLightEvent();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in HandleClientComm: " + ex.Message);
                    break;
                }
            }

            // Clean-up after client disconnects
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
        }

        //private void HandleClientCommOLD()
        //{
        //    while (IsListening && client.Connected)
        //    {
        //        try
        //        {
        //            byte[] buffer = new byte[1024];
        //            int bytesRead = stream.Read(buffer, 0, buffer.Length);
        //            if (bytesRead == 0) break;  // Connection closed

        //            // message structure
        //            string receivedSimData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        //            Console.WriteLine("< Client" + receivedSimData);

        //            // Check for specific commands to send to SimConnect
        //            if (receivedSimData.Contains("LIGHT_TAXI_ON"))
        //            {
        //                simConnection.SendTaxiLightEvent();
        //            }

        //            // Acquire data to send
        //            if (DataQueues.TryDequeueReceive(out string dataToSend))
        //            {
        //                //string response = "From sim: " + dataToSend;
        //                string response = dataToSend;
        //                byte[] data = Encoding.UTF8.GetBytes(response);
        //                stream.Write(data, 0, data.Length);
        //                Console.WriteLine("> Client" + data);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Error in HandleClientComm: " + ex.Message);
        //            break;
        //        }
        //    }

        //    // Clean-up after client disconnects
        //    if (stream != null)
        //        stream.Close();
        //    if (client != null)
        //        client.Close();
        //}

        public void ReceiveMessage(Message m)
        {
            if (simConnection != null)
            {
                simConnection.ReceiveMessage(m);
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
