using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using Microsoft.FlightSimulator.SimConnect;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MSFServer
{
    internal class SimConnection
    {
        private Thread simThread;
        private bool isRunning;
        private SimConnect simConnect;
        private const int WM_USER_SIMCONNECT = 0x0402;

        public void Start()
        {
            isRunning = true;
            simThread = new Thread(SimConnectLoop);
            simThread.IsBackground = true;
            simThread.Start();
        }

        public void Stop()
        {
            isRunning = false;
            if (simThread != null && simThread.IsAlive)
                simThread.Join();
        }

        private void SimConnectLoop()
        {
            // Initialize SimConnect
            InitializeSimConnect();

            while (isRunning)
            {
                // Handle SimConnect events
                simConnect.ReceiveMessage();

                //// Send the taxi light event for testing
                //SendTaxiLightEvent();

                // Sleep or wait for a specific interval
                Thread.Sleep(1000);
            }

            // Old testing loop
            //while (isRunning)
            //{
            //    // Handle data to be sent to SimConnect
            //    if (DataQueues.TryDequeueSend(out string dataToSend))
            //    {
            //        SendDataToSim(dataToSend);
            //    }

            //    // Handle data received from SimConnect
            //    //Fictitional data:
            //    string receivedData = "Sim sent: " + DateTime.Now.ToString("HH:mm:ss");
            //    //byte[] data = Encoding.UTF8.GetBytes(message);
            //    //stream.Write(data, 0, data.Length);

            //    //string receivedData = GetSimData();
            //    DataQueues.EnqueueReceive(receivedData);
            //    Console.WriteLine(receivedData);
            //    // Sleep or wait for a specific interval
            //    Thread.Sleep(1000);
            //}
        }

        private void InitializeSimConnect()
        {
            try
            {
                simConnect = new SimConnect("Managed Data Request", IntPtr.Zero, WM_USER_SIMCONNECT, null, 0);
                
                // Define the data structure
                simConnect.AddToDataDefinition(DEFINITIONS.Struct1, "PLANE ALTITUDE", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);

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

                // Map the event ID to the SimConnect event
                simConnect.MapClientEventToSimEvent(EVENTS.LIGHT_TAXI, "TOGGLE_TAXI_LIGHTS");

                Console.WriteLine("SimConnect Initialized Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error initializing SimConnect: " + ex.Message);
            }
        }

        private void SimConnect_OnRecvSimobjectData(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA data)
        {
            if ((DATA_REQUESTS)data.dwRequestID == DATA_REQUESTS.Request1)
            {
                Struct1 receivedData = (Struct1)data.dwData[0];
                string message = "Sim sent: Altitude = " + receivedData.Altitude;

                // Enqueue received data
                DataQueues.EnqueueReceive(message);

                Console.WriteLine("Received data from SimConnect: " + message);
            }
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

        // Define the SIMCONNECT_GROUP_PRIORITY enum
        enum SIMCONNECT_GROUP_PRIORITY
        {
            HIGHEST = 1,
            HIGH = 100,
            STANDARD = 1000,
            LOW = 1900,
            LOWEST = 2000,
        }


        // Old placeholders
        //private void SendDataToSim(string data)
        //{
        //    // Implement the logic to send data to SimConnect
        //    Console.WriteLine("Sent to SimConnect: " + data);
        //}

        //private string GetSimData()
        //{
        //    // Implement the logic to communicate with SimConnect and get data
        //    return "Simulated Data from SimConnect";
        //}


    }
}
