using System;
using System.Collections.Concurrent;

namespace MSFServer
{
    public static class DataQueues
    {
        public static ConcurrentQueue<string> SimReceiveQueue = new ConcurrentQueue<string>();
        public static ConcurrentQueue<string> SimSendQueue = new ConcurrentQueue<string>();

        // Define the event
        public static event Action<string> SimDataReceived;

        public static void EnqueueReceive(string data)
        {
            SimReceiveQueue.Enqueue(data);
            SimDataReceived?.Invoke(data); // Raise the event

            //Console.WriteLine($"Queue Size: {SimReceiveQueue.Count}");
        }

        public static bool TryDequeueReceive(out string data)
        {
            return SimReceiveQueue.TryDequeue(out data);
        }

        public static void EnqueueSend(string data)
        {
            SimSendQueue.Enqueue(data);
        }

        public static bool TryDequeueSend(out string data)
        {
            return SimSendQueue.TryDequeue(out data);
        }
    }
}
