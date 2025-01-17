//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace MSFServer
{
    public static class DataQueues
    {
        public static ConcurrentQueue<string> SimReceiveQueue = new ConcurrentQueue<string>();
        public static ConcurrentQueue<string> SimSendQueue = new ConcurrentQueue<string>();

        public static void EnqueueReceive(string data)
        {
            SimReceiveQueue.Enqueue(data);
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
