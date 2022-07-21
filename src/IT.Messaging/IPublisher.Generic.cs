using System;

namespace IT.Messaging;

public interface IPublisher<T> : IAsyncPublisher<T>
{
    void Publish(String channel, T message);
}