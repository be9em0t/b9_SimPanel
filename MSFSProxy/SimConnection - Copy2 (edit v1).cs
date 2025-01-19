using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using Microsoft.FlightSimulator.SimConnect;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Reflection;

namespace MSFServer
{
    internal class SimConnection
    {
        //private Thread simThread;
        //private bool isRunning;
        private SimConnect simConnect;
        private const int WM_USER_SIMCONNECT = 0x0402;
        private Dictionary<string, object> previousValues = new Dictionary<string, object>();


        public void Start()
        {
            //isRunning = true;
            //simThread = new Thread(SimConnectLoop);
            //simThread.IsBackground = true;
            //simThread.Start();
            InitializeSimConnect();
        }

        public void Stop()
        {
            // Clean up SimConnect resources
            if (simConnect != null)
            {
                simConnect.Dispose();
                simConnect = null;
            }
        }

        //private void SimConnectLoop()
        //{
        //    // Initialize SimConnect
        //    InitializeSimConnect();

        //    while (isRunning)
        //    {
        //        simConnect.ReceiveMessage();

        //        Thread.Sleep(1000);
        //    }
        //}


        private void InitializeSimConnect()
        {
            try
            {
                simConnect = new SimConnect("Managed Data Request", IntPtr.Zero, WM_USER_SIMCONNECT, null, 0);

                // Define the data structure
                simConnect.AddToDataDefinition(DEFINITIONS.Struct1, "PLANE ALTITUDE", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simConnect.AddToDataDefinition(DEFINITIONS.Struct1, "LIGHT LANDING", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simConnect.AddToDataDefinition(DEFINITIONS.Struct1, "LIGHT TAXI", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simConnect.AddToDataDefinition(DEFINITIONS.Struct1, "LIGHT BEACON", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);

                // Register the data definition
                simConnect.RegisterDataDefineStruct<Struct1>(DEFINITIONS.Struct1);

                // Request data on a defined interval with the required parameters
                simConnect.RequestDataOnSimObject(
                    DATA_REQUESTS.Request1,
                    DEFINITIONS.Struct1,
                    SimConnect.SIMCONNECT_OBJECT_ID_USER,
                    SIMCONNECT_PERIOD.SECOND,
                    SIMCONNECT_DATA_REQUEST_FLAG.DEFAULT,
                    0, 0, 0);

                // Assign the receive handler
                simConnect.OnRecvSimobjectData += SimConnect_OnRecvSimobjectData;
                simConnect.OnRecvOpen += SimConnect_OnRecvOpen;
                simConnect.OnRecvQuit += SimConnect_OnRecvQuit;

                // Map the event ID to the SimConnect event
                simConnect.MapClientEventToSimEvent(EVENTS.LIGHT_TAXI, "TOGGLE_TAXI_LIGHTS");

                Console.WriteLine("SimConnect Initialized Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error initializing SimConnect: " + ex.Message);
            }
        }

        private void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            Console.WriteLine("Connected to Flight Simulator");
        }

        private void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            Console.WriteLine("Flight Simulator has exited");
            Stop();
        }

        private void SimConnect_OnRecvSimobjectData(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA data)
        {
            Console.WriteLine("OnRecvSimobjectData invoked");
            if ((DATA_REQUESTS)data.dwRequestID == DATA_REQUESTS.Request1)
            {
                Struct1 receivedData = (Struct1)data.dwData[0];

                // Convert struct to dictionary for comparison
                var receivedDataDict = StructToDictionary(receivedData);

                List<string> changedVariables = DetectChanges(receivedDataDict);

                if (changedVariables.Count > 0)
                {
                    string message = "Sim sent: " + string.Join(", ", changedVariables);
                    DataQueues.EnqueueReceive(message);
                    Console.WriteLine("Received data from SimConnect: " + message);
                }
            }
        }

        private Dictionary<string, object> StructToDictionary<T>(T structObj)
        {
            var dict = new Dictionary<string, object>();
            foreach (FieldInfo field in structObj.GetType().GetFields())
            {
                dict[field.Name] = field.GetValue(structObj);
            }
            return dict;
        }

        private List<string> DetectChanges(Dictionary<string, object> receivedDataDict)
        {
            List<string> changedVariables = new List<string>();

            foreach (var kvp in receivedDataDict)
            {
                string key = kvp.Key;
                object value = kvp.Value;

                if (!previousValues.ContainsKey(key) || !Equals(previousValues[key], value))
                {
                    previousValues[key] = value;
                    changedVariables.Add($"{key} = {value}");
                }
            }

            return changedVariables;
        }

        public void SendTaxiLightEvent()
        {
            try
            {
                simConnect.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, EVENTS.LIGHT_TAXI, 0, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                Console.WriteLine("Taxi light event sent.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending taxi light event: " + ex.Message);
            }
        }

        // Define the structure
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct Struct1
        {
            public double Altitude;
            public int LandingLight;
            public int TaxiLight;
            public int BeaconLight;
        }

        // Define enums
        enum DEFINITIONS
        {
            Struct1,
        }

        enum DATA_REQUESTS
        {
            Request1,
        }

        enum EVENTS
        {
            LIGHT_TAXI,
        }

        enum SIMCONNECT_GROUP_PRIORITY
        {
            HIGHEST = 1,
            HIGH = 100,
            STANDARD = 1000,
            LOW = 1900,
            LOWEST = 2000,
        }

    }
}
