using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Data.SqlClient;
using MSFSClient;

namespace MSFServer
{
    public partial class FormProxy : Form
    {
        private IniFile iniFile;
        private ConnectionManager connectionManager;
        private bool IsSimAllowed_check;
        private const int WM_USER_SIMCONNECT = 0x0402; // Custom window message for SimConnect

        public FormProxy()
        {
            InitializeComponent();
            textBoxIP.Text = ConnectionManager.GetLocalIPv4();
            iniFile = new IniFile("Settings.ini");
            textBoxPort.Text = iniFile.Read("Settings", "Port");
            checkBox_Sim.Checked = iniFile.Read("Settings", "EnableSim") == "True";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProxy_FormClosing);

            button_Serve.Click += new EventHandler(button_Serve_Click);
        }

        private void button_Serve_Click(object sender, EventArgs e)
        {
            if (connectionManager == null || !connectionManager.IsListening)
            {
                // Start the server
                connectionManager = new ConnectionManager(this.Handle, int.Parse(textBoxPort.Text)); // Pass the handle here
                connectionManager.IsSimAllowed = checkBox_Sim.Checked;
                connectionManager.Start();
                button_Serve.Text = "Stop";
            }
            else
            {
                // Stop the server
                connectionManager.Stop();
                button_Serve.Text = "Start";
            }
        }

        private void FormProxy_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save settings on close
            iniFile.Write("Settings", "IPAddress", textBoxIP.Text);
            iniFile.Write("Settings", "Port", textBoxPort.Text);
            iniFile.Write("Settings", "EnableSim", checkBox_Sim.Checked.ToString());
            Console.WriteLine("Settings.ini updated");
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_USER_SIMCONNECT)
            {
                if (connectionManager != null && connectionManager.IsListening) // Add IsListening check
                {
                    connectionManager.ReceiveMessage(m);
                }
            }
            base.WndProc(ref m);
        }

    }
}