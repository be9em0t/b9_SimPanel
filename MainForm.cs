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
    public partial class MainForm : Form
    {
        private SimConnect simconnect;
        private const int WM_USER_SIMCONNECT = 0x0402;

        public MainForm()
        {
            InitializeComponent();
            InitializeSimConnect();
            
        }

        enum EventID
        {
            EVENT_LIGHT_TAXI = 0x11000,
            EVENT_LIGHT_BEACON = 0x11001
        }

        enum GroupPriority
        {
            HIGHEST = 1
        }

        private void InitializeSimConnect()
        {
            try
            {
                simconnect = new SimConnect("MyApp", this.Handle, WM_USER_SIMCONNECT, null, 0);
                Console.WriteLine("Connected");
                Console.WriteLine("Connected");
                Console.WriteLine("Connected");
                Console.WriteLine("Connected");
                Console.WriteLine("Connected");
                SetLightValues();
            }
            catch (COMException ex)
            {
                MessageBox.Show("Failed to connect: " + ex.Message);
            }
        }

        private void SetLightValues()
        {
            const uint LIGHT_ON = 1;

            // Transmit the event to turn on the taxi light
            simconnect.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, EventID.EVENT_LIGHT_TAXI, LIGHT_ON, GroupPriority.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);

            // Transmit the event to turn on the beacon light
            simconnect.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, EventID.EVENT_LIGHT_BEACON, LIGHT_ON, GroupPriority.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
            Console.WriteLine("Tried to send...");
        }

        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg == WM_USER_SIMCONNECT)
            {
                if (simconnect != null)
                {
                    simconnect.ReceiveMessage();
                }
            }
            else
            {
                base.DefWndProc(ref m);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (simconnect != null)
            {
                Console.WriteLine("Disconnecting...");
                simconnect.Dispose();
            }
        }
    }
}
