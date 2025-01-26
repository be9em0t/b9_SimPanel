using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using Microsoft.FlightSimulator.SimConnect;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

namespace MSFServer
{
    internal class SimConnection
    {
        private SimConnect simConnect;
        private const int WM_USER_SIMCONNECT = 0x0402;
        private Dictionary<string, object> previousValues = new Dictionary<string, object>();
        private int IsSimRawData;

        public void Initialize(IntPtr windowHandle)
        {
            try
            {
                simConnect = new SimConnect("Managed Data Request", windowHandle, WM_USER_SIMCONNECT, null, 0);

                simConnect.AddToDataDefinition(DEFINITIONS.Struct1, "PLANE ALTITUDE", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                //lights
                simConnect.AddToDataDefinition(DEFINITIONS.Struct1, "LIGHT LANDING", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simConnect.AddToDataDefinition(DEFINITIONS.Struct1, "LIGHT TAXI", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simConnect.AddToDataDefinition(DEFINITIONS.Struct1, "LIGHT BEACON", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);

                // simConnect.AddToDataDefinition(DEFINITIONS.Struct1, "LIGHT STROBE", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                // simConnect.AddToDataDefinition(DEFINITIONS.Struct1, "LIGHT NAV", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                // simConnect.AddToDataDefinition(DEFINITIONS.Struct1, "LIGHT WING", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                // simConnect.AddToDataDefinition(DEFINITIONS.Struct1, "LIGHT RECOGNITION", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                // simConnect.AddToDataDefinition(DEFINITIONS.Struct1, "LIGHT LOGO", "bool", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);

                simConnect.RegisterDataDefineStruct<Struct1>(DEFINITIONS.Struct1);

                simConnect.OnRecvSimobjectData += SimConnect_OnRecvSimobjectData;
                simConnect.OnRecvOpen += SimConnect_OnRecvOpen;
                simConnect.OnRecvQuit += SimConnect_OnRecvQuit;
                simConnect.MapClientEventToSimEvent(EVENTS.LIGHT_TAXI, "TOGGLE_TAXI_LIGHTS");
                Console.WriteLine("SimConnect Initialized Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error initializing SimConnect: " + ex.Message);
            }
        }

        public void Start(bool simRawData)
        {
            IsSimRawData = simRawData ? 1 : 0;
            if (simConnect != null)
            {
                simConnect.RequestDataOnSimObject(
                    DATA_REQUESTS.Request1,
                    DEFINITIONS.Struct1,
                    SimConnect.SIMCONNECT_OBJECT_ID_USER,
                    SIMCONNECT_PERIOD.SECOND,
                    SIMCONNECT_DATA_REQUEST_FLAG.DEFAULT,
                    0, 0, 0);
            }
            else Console.WriteLine("Simconnect not available");
        }

        public void Stop()
        {
            if (simConnect != null)
            {
                simConnect.Dispose();
                simConnect = null;
            }
        }

        public void ReceiveMessage(Message m)
        {
            simConnect.ReceiveMessage();
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
            if ((DATA_REQUESTS)data.dwRequestID == DATA_REQUESTS.Request1)
            {
                //Console.WriteLine("SimConnect_OnRecvSimobjectData");
                Struct1 receivedSimData = (Struct1)data.dwData[0];

                // Convert struct to dictionary for comparison
                var receivedSimDataDict = StructToDictionary(receivedSimData);

                //Console.WriteLine(IsSimRawData);
                var simDataDict = DetectChangesDict(receivedSimDataDict, IsSimRawData);
                //Console.WriteLine(simDataDict.Keys.First());

                if (simDataDict.Count > 0)
                {
                    // Serialize the dictionary to JSON
                    string simDataJson = JsonConvert.SerializeObject(simDataDict);

                    DataQueues.EnqueueReceive(simDataJson);
                    //Console.WriteLine("< " + simDataJson);
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

        private Dictionary<string, object> DetectChangesDict(Dictionary<string, object> receivedSimDataDict, int raw)
        {
            var variablesDict = new Dictionary<string, object>();
            variablesDict["< RawSim>"] = raw;

            if (raw == 0)
            {
                foreach (var kvp in receivedSimDataDict)
                {
                    string key = kvp.Key;
                    object value = kvp.Value;

                    if (!previousValues.ContainsKey(key) || !Equals(previousValues[key], value))
                    {
                        previousValues[key] = value;
                        variablesDict[key] = value;
                    }
                }
            }
            else {
                variablesDict = receivedSimDataDict;
            }

            return variablesDict;
        }

        public void SendTaxiLightEvent()
        {
            try
            {
                simConnect.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, EVENTS.LIGHT_TAXI, 0, SIMCONNECT_GROUP_PRIORITY.HIGHEST, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
                Console.WriteLine("> Sim: LIGHT_TAXI");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error > Sim: LIGHT_TAXI: " + ex.Message);
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct Struct1
        {
            public double Altitude;
            public int LandingLight;
            public int TaxiLight;
            public int BeaconLight;
        }

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
