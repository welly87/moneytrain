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
                string pubMessage = string.Format("{0}:{1}", parts[1], parts[2]);

                _dequeueReceiver.Send(pubMessage, Encoding.Unicode);

                _commandReceiver.Send("Success", Encoding.Unicode);
            }

            Console.WriteLine(message);
        }

        public void Stop()
        {
            _running = false;
            _commandReceiver.Close();
            _context.Dispose();
        }
    }
}