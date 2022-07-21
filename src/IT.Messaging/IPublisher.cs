using System;

namespace IT.Messaging;

public interface IPublisher : IAsyncPublisher
{
    void Publish<T>(String channel, T message);

    void Publish(String channel, ReadOnlyMemory<Byte> message);
}