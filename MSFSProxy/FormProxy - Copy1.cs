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

        public FormProxy()
        {
            InitializeComponent();
            textBoxIP.Text = ConnectionManager.GetLocalIPv4();
            iniFile = new IniFile("Settings.ini");
            // Load settings on startup
            //textBoxIP.Text = iniFile.Read("Settings", "IPAddress");
            textBoxPort.Text = iniFile.Read("Settings", "Port");
            checkBox_Sim.Checked = iniFile.Read("Settings", "EnableSim") == "True";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProxy_FormClosing);

            button_Serve.Click += new EventHandler(button_Serve_Click);
            //checkBox_Sim.CheckedChanged += new EventHandler(checkBoxEnableSim_CheckedChanged);

        }

        private void button_Serve_Click(object sender, EventArgs e)
        {
            if (connectionManager == null || !connectionManager.IsListening)
            {
                // Start the server
                connectionManager = new ConnectionManager(int.Parse(textBoxPort.Text));
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

    }
}