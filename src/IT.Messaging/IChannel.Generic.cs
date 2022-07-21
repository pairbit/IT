namespace IT.Messaging;

public interface IChannel<T> : IAsyncChannel<T>, IPublisher<T>, ISubscriber<T>
{

}