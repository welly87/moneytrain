namespace MoneyTrain.Buzz
{
    public interface IHandleMessages<T> where T : IMessage
    {
        void Handle(T message);
    }
}