using System;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace MoneyTrain.Buzz
{
    public class MessageBus
    {
        private readonly string _endpointName;
        private readonly SubscriptionStorage _subscriptionStorage = new SubscriptionStorage();
        private ZmqContext _context;
        private ZmqSocket _listener;
        private bool _running;

        public MessageBus(string endpointName)
        {
            _endpointName = endpointName;
        }

        public void Send(IMessage message, string destination)
        {
            SendMessageToMq(string.Format("{0}:{1}:{2}", "Enqueue", destination, message));
        }

        private void SendMessageToMq(string message)
        {
            ZmqSocket sender = _context.CreateSocket(SocketType.REQ);
            sender.Connect("tcp://localhost:7777");
            sender.Send(message, Encoding.Unicode);
            string response = sender.Receive(Encoding.Unicode);
            Console.WriteLine(response);
            sender.Close();
            sender.Dispose();
        }

        public void Start()
        {
            _context = ZmqContext.Create();
            //CreateEndpointIfNotExists();
            ListenToMessageFromQueue();
        }

        public void Stop()
        {
            _running = false;
            _context.Dispose();
        }

        private void ListenToMessageFromQueue()
        {
            _running = true;

            _listener = _context.CreateSocket(SocketType.SUB);
            _listener.Subscribe(Encoding.Unicode.GetBytes(_endpointName));

            _listener.Connect("tcp://localhost:7778");

            Task.Run(() =>
                {
                    while (_running)
                    {
                        string messages = DequeueMessages();

                        //if (messages.Count == 0) continue;

                        ProcessMessage(messages);
                        //Thread.Sleep(10);
                    }
                });
        }

        private void ProcessMessage(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            if (IsSubscriptionMessage(message))
            {
                HandleSubsriptionMessage(message);
            }

            Console.WriteLine(message);
        }

        private void HandleSubsriptionMessage(string message)
        {
            string[] parts = message.Split('=');
            _subscriptionStorage.Register(parts[1], parts[2]);
        }

        private bool IsSubscriptionMessage(string message)
        {
            return message.StartsWith("Subscribe");
        }

        private string DequeueMessages()
        {
            string raw = _listener.Receive(Encoding.Unicode);

            return ParseMessage(raw);
        }

        private string ParseMessage(string raw)
        {
            return raw.Substring(_endpointName.Length + 1);
        }

        //private void CreateEndpointIfNotExists()
        //{
        //    SendMessageToMq("CreateEndpoint:" + _endpointName);
        //}

        public void Publish(IEvent @event)
        {
            foreach (string subscriber in _subscriptionStorage.GetAllSubscriberForMessageType(@event.GetType()))
            {
                Send(@event, subscriber);
            }
        }

        public void Subscribe<T>(string endpoint) where T : IEvent
        {
            SendSubscriptionMessageTo(typeof (T).ToString(), endpoint);
        }

        private void SendSubscriptionMessageTo(string messageType, string endpoint)
        {
            string message = CreateSubscriptionMessage(messageType, _endpointName);
            SendMessageToMq(string.Format("{0}:{1}:{2}", "Enqueue", endpoint, message));
        }

        private string CreateSubscriptionMessage(string messageType, string subscriber)
        {
            return string.Format("Subscribe={0}={1}", subscriber, messageType);
        }
    }
}