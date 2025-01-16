using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Threading.Tasks;


namespace MSFServer
{
    public partial class FormProxy : Form
    {
        public FormProxy()
        {
            InitializeComponent();
            textBoxIP.Text = GetLocalIPv4();
            textBoxPort.Text = "505";
            button_Serve.Click += new EventHandler(button_Serve_Click);
        }

        private void button_Serve_Click(object sender, EventArgs e)
        {
            Serve();
        }

        private void Serve()
        {
            int port;
            if (!int.TryParse(textBoxPort.Text, out port))
            {
                MessageBox.Show("Please enter a valid port number.");
                return;
            }

            TcpListener server = new TcpListener(IPAddress.Any, port);
            server.Start();
            Console.WriteLine("Server started...");

            while (true)
            {
                try
                {
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();

                    // Receive Data
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received: " + receivedData);

                    //// Deserialize data
                    var deserializedObject = JsonConvert.DeserializeObject(receivedData);
                    Console.WriteLine("Deserialized object: " + deserializedObject);

                    stream.Close();
                    client.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

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
