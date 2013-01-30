using System;
using MoneyTrain.Buzz;
using MoneyTrain.ServiceMessage;

namespace MoneyTrain.ServiceA
{
    internal class Program
    {
        private static void Main()
        {
            var bus = new MessageBus("ServiceA");
            bus.Start();

            bus.Subscribe<TradeOpportunityFound>("Publisher");

            Console.ReadLine();

            for (int i = 0; i < 100; i++)
            {
                bus.Send(new SimpleMessage { Data = "Hello World" }, "ServiceB");
            }

            Console.ReadLine();
        }
    }
}