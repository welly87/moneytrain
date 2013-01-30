using MoneyTrain.Buzz;
using MoneyTrain.ServiceMessage;

namespace MoneyTrain.ServiceB
{
    public class SimpleMessageHandler : IHandleMessages<SimpleMessage>
    {
        public void Handle(SimpleMessage message)
        {
        }
    }
}