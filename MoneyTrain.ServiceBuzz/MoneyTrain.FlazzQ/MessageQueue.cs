using System.Collections.Concurrent;

namespace MoneyTrain.FlazzQ
{
    public class MessageQueue
    {
        private readonly ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();

        internal void Enqueue(string message)
        {
            _queue.Enqueue(message);
        }

        public string Dequeue()
        {
            string result;

            _queue.TryDequeue(out result);

            return result ?? string.Empty;
        }
    }
}