using System;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace MoneyTrain.FlazzQ
{
    public class FlazzQueue
    {
        private ZmqSocket _commandReceiver;
        private ZmqContext _context;
        private ZmqSocket _dequeueReceiver;
        private bool _running;

        public void Start()
        {
            _context = ZmqContext.Create();
            _commandReceiver = _context.CreateSocket(SocketType.REP);
            _commandReceiver.Bind("tcp://*:7777");

            _dequeueReceiver = _context.CreateSocket(SocketType.PUB);
            _dequeueReceiver.Bind("tcp://*:7778");

            StartListener();
        }

        private void StartListener()
        {
            _running = true;

            Task.Run(() =>
                {
                    while (_running)
                    {
                        string message = _commandReceiver.Receive(Encoding.Unicode);
                        HandleMessage(message);
                    }
                });
        }

        private void HandleMessage(string message)
        {
            // should serialize message and then dispatch

            if (message.StartsWith("Enqueue"))
            {
                string[] parts = message.Split(':');
                //EnqueueMessageToQueue(parts[1], parts[2]);
                string pubMessage = string.Format("{0}:{1}", parts[1], parts[2]);

                _dequeueReceiver.Send(pubMessage, Encoding.Unicode);

                _commandReceiver.Send("Success", Encoding.Unicode);
            }

            //if (message.StartsWith("CreateEndpoint"))
            //{
            //    string[] parts = message.Split(':');
            //    CreateQueueIfNotExists(parts[1]);

            //    _commandReceiver.Send("Success", Encoding.Unicode);
            //}

            //else if (message.StartsWith("Dequeue"))
            //{
            //    string[] parts = message.Split(':');

            //    _commandReceiver.Send(DequeueMessages(parts[1]), Encoding.Unicode);
            //}
            Console.WriteLine(message);
        }

        //private string DequeueMessages(string queue)
        //{
        //    // should loop all messages, not just one
        //    return _messageQueues[queue].Dequeue();
        //}

        //private void EnqueueMessageToQueue(string queue, string message)
        //{
        //    // check if the message is local or another server
        //    _messageQueues[queue].Enqueue(message);
        //}

        //private void CreateQueueIfNotExists(string queue)
        //{
        //    if (queue == null) return;

        //    Console.WriteLine("Create Queue : " + queue);
        //    _messageQueues.TryAdd(queue, new MessageQueue());
        //}

        public void Stop()
        {
            _running = false;
            _commandReceiver.Close();
            _context.Dispose();
        }

        //public void CreateQueue(string queue)
        //{
        //    CreateQueueIfNotExists(queue);
        //}
    }
}