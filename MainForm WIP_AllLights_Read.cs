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
//namespace SimleSimvarTest
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
            public int Light_Cabin_Status;
            public int Light_Landing_Status;
            public int Light_Logo_Status;
            public int Light_Nav_Status;
            public int Light_Panel_Status;
            public int Light_Recognition_Status;
            public int Light_Strobe_Status;
            public int Light_Wing_Status;
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

        private void InitializeSimConnect()
        {
            try
            {
                simconnect = new SimConnect("MyApp", this.Handle, 0x0402, null, 0);
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
            simconnect.AddToDataDefinition(DEFINITIONS.LightStatusData, "LIGHT CABIN", "", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            simconnect.AddToDataDefinition(DEFINITIONS.LightStatusData, "LIGHT LANDING", "", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            simconnect.AddToDataDefinition(DEFINITIONS.LightStatusData, "LIGHT LOGO", "", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            simconnect.AddToDataDefinition(DEFINITIONS.LightStatusData, "LIGHT NAV", "", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            simconnect.AddToDataDefinition(DEFINITIONS.LightStatusData, "LIGHT PANEL", "", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            simconnect.AddToDataDefinition(DEFINITIONS.LightStatusData, "LIGHT RECOGNITION", "", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            simconnect.AddToDataDefinition(DEFINITIONS.LightStatusData, "LIGHT STROBE", "", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            simconnect.AddToDataDefinition(DEFINITIONS.LightStatusData, "LIGHT WING", "", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
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
                Console.WriteLine("LIGHT CABIN: " + LightStatusData.Light_Cabin_Status);
                Console.WriteLine("LIGHT LANDING: " + LightStatusData.Light_Landing_Status);
                Console.WriteLine("LIGHT LOGO: " + LightStatusData.Light_Logo_Status);
                Console.WriteLine("LIGHT NAV: " + LightStatusData.Light_Nav_Status);
                Console.WriteLine("LIGHT PANEL: " + LightStatusData.Light_Panel_Status);
                Console.WriteLine("LIGHT RECOGNITION: " + LightStatusData.Light_Recognition_Status);
                Console.WriteLine("LIGHT STROBE: " + LightStatusData.Light_Strobe_Status);
                Console.WriteLine("LIGHT WING: " + LightStatusData.Light_Wing_Status);
            }
        }

        private void TaxiLightsToggle_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Toggle Click!");
            Console.WriteLine("\nToggle Click!\n");
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
