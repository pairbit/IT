namespace IT.Messaging;

public interface IAsyncChannel<T> : IAsyncPublisher<T>, IAsyncSubscriber<T>
{

}