using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Data.SqlClient;

namespace MSFServer
{
    public partial class FormProxy : Form
    {
        private ConnectionManager connectionManager;
        private bool IsSimAllowed_check;

        public FormProxy()
        {
            InitializeComponent();
            textBoxIP.Text = ConnectionManager.GetLocalIPv4();
            textBoxPort.Text = "505";
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

    }
}