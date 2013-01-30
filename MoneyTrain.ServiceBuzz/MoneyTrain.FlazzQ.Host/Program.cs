using System;

namespace MoneyTrain.FlazzQ.Host
{
    internal class Program
    {
        private static void Main()
        {
            var flazzQ = new FlazzQueue();
            //flazzQ.CreateQueue("Publisher");
            //flazzQ.CreateQueue("ServiceA");
            //flazzQ.CreateQueue("ServiceB");
            flazzQ.Start();

            Console.WriteLine("Flazz Queue is ready...");

            Console.ReadLine();
        }
    }
}