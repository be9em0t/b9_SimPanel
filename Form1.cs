using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.FlightSimulator.SimConnect;
using System.Runtime.InteropServices;

namespace b9_SimPanel
{
    public partial class Form1 : Form
    {
        private SimConnect simConnect = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void labelConnect_Click(object sender, EventArgs e)
        {

        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                simConnect = new SimConnect(
                    "SimConnect Example App",  // App name
                    this.Handle,              // Handle to the form
                    0,                        // Client event ID (0 for default)
                    null,                     // Event handle (not needed here)
                    0                         // Configuration index (0 for default)
                );
                simConnect.OnRecvOpen += SimConnect_OnRecvOpen;
                simConnect.OnRecvQuit += SimConnect_OnRecvQuit;

                UpdateStatus("Connected to MSFS!");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Connection failed: {ex.Message}");
            }
        }

        private void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            UpdateStatus("SimConnect connection opened!");
        }

        private void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            UpdateStatus("MSFS has closed. Disconnecting...");
            DisconnectSimConnect();
        }

        private void DisconnectSimConnect()
        {
            simConnect?.Dispose();
            simConnect = null;
            UpdateStatus("Disconnected from MSFS.");
        }

        private void UpdateStatus(string message)
        {
            // Update UI label for status
            statusLabel.Text = message;
        }

        protected override void DefWndProc(ref Message m)
        {
            if (simConnect != null)
            {
                simConnect.ReceiveMessage();
            }
            base.DefWndProc(ref m);
        }

    }
}
