using System;

namespace IT.PubSub;

public interface IPublisher : IAsyncPublisher
{
    Int64 Publish(String channel, String message);

    Int64 Publish(String channel, Int64 message);

    Int64 Publish(String channel, UInt64 message);

    Int64 Publish(String channel, Double message);

    Int64 Publish(String channel, Byte[] message);

    Int64 Publish(String channel, ReadOnlyMemory<Byte> message);
}