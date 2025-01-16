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

        [StructLayout(LayoutKind.Sequential)]
        public struct LightStatusData
        {
            public int Light_Taxi_Status;
            public int Light_Beacon_Status;
        }

        public MainForm()
        {
            InitializeComponent();
            InitializeSimConnect();
            // Add an event handler for the ImageBox click event
            TaxiLightsToggle.Click += TaxiLightsToggle_Click;
        }

        enum DEFINITIONS
        {
            LightStatusData,
        }

        enum DATA_REQUESTS
        {
            RequestLightStatus,
        }

        enum EventID //events for setting 
        {
            EVENT_LIGHT_TAXI = 0x11000,
            EVENT_LIGHT_BEACON = 0x11001
        }

        enum GroupPriority //variables for setting 
        {
            HIGHEST = 1
        }

        private void InitializeSimConnect()
        {
            try
            {
                simconnect = new SimConnect("MyApp", this.Handle, WM_USER_SIMCONNECT, null, 0);
                simconnect.OnRecvSimobjectData += OnDataReceived;
                Console.WriteLine("Connected");
                SubscribeToLightStatus();
            }
            catch (COMException ex)
            {
                MessageBox.Show("Failed to connect: " + ex.Message);
            }
        }

        private void SubscribeToLightStatus()
        {
            simconnect.AddToDataDefinition(DEFINITIONS.LightStatusData, "LIGHT TAXI", "", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            simconnect.AddToDataDefinition(DEFINITIONS.LightStatusData, "LIGHT BEACON", "", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            simconnect.RegisterDataDefineStruct<LightStatusData>(DEFINITIONS.LightStatusData);
            simconnect.RequestDataOnSimObject(DATA_REQUESTS.RequestLightStatus, DEFINITIONS.LightStatusData, SimConnect.SIMCONNECT_OBJECT_ID_USER, SIMCONNECT_PERIOD.SECOND, 0, 0, 0, 0);
            Console.WriteLine("Requested Lights data.");
        }

        private void OnDataReceived(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA data)
        {
            if (data.dwRequestID == (uint)DATA_REQUESTS.RequestLightStatus)
            {
                LightStatusData LightStatusData = (LightStatusData)data.dwData[0];
                Console.WriteLine("LIGHT TAXI: " + LightStatusData.Light_Taxi_Status);
                Console.WriteLine("LIGHT BEACON: " + LightStatusData.Light_Beacon_Status);
                Console.WriteLine("\n");
            }
        }

        private void TaxiLightsToggle_Click(object sender, EventArgs e)
        {
            // Toggle the light state
            // ToggleLight(EventID.EVENT_LIGHT_TAXI);
            // ToggleLight(EventID.EVENT_LIGHT_BEACON);
            ToggleLight(EventID.EVENT_LIGHT_TAXI, LightStatusData.Light_Taxi_Status);
            ToggleLight(EventID.EVENT_LIGHT_BEACON, LightStatusData.Light_Beacon_Status);
            MessageBox.Show("Toggle Click!");
        }

        //function for setting
        private void ToggleLight(EventID eventID, int currentStatus)
        {
            // Toggle the light status
            int newStatus = (currentStatus == 0) ? 1 : 0;

            // Transmit the event to change the light state
            simconnect.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, eventID, (uint)newStatus, GroupPriority.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
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
