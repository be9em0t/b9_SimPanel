using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

namespace MSFServer
{
    internal class SimConnection
    {
        private Thread simThread;
        private bool isRunning;

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
            while (isRunning)
            {
                // Handle data to be sent to SimConnect
                if (DataQueues.TryDequeueSend(out string dataToSend))
                {
                    SendDataToSim(dataToSend);
                }

                // Handle data received from SimConnect
                //Fictitional data:
                string receivedData = "Sim sent: " + DateTime.Now.ToString("HH:mm:ss");
                //byte[] data = Encoding.UTF8.GetBytes(message);
                //stream.Write(data, 0, data.Length);

                //string receivedData = GetSimData();
                DataQueues.EnqueueReceive(receivedData);
                Console.WriteLine(receivedData);
                // Sleep or wait for a specific interval
                Thread.Sleep(1000);
            }
        }

        private void SendDataToSim(string data)
        {
            // Implement the logic to send data to SimConnect
            Console.WriteLine("Sent to SimConnect: " + data);
        }

        private string GetSimData()
        {
            // Implement the logic to communicate with SimConnect and get data
            return "Simulated Data from SimConnect";
        }


    }
}
