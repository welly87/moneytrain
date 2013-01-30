using System;
using MoneyTrain.Buzz;
using MoneyTrain.ServiceMessage;

namespace MoneyTrain.Publisher
{
    internal class Program
    {
        private static void Main()
        {
            var bus = new MessageBus("Publisher");
            bus.Start();

            //for (int i = 0; i < 100; i++)
            //{
            //    bus.Send(new SimpleMessage {Data = "Hello World"}, "ServiceB");
            //}

            Console.ReadLine();

            for (int i = 0; i < 100; i++)
            {
                bus.Publish(new TradeOpportunityFound());
            }


            Console.ReadLine();
        }
    }
}