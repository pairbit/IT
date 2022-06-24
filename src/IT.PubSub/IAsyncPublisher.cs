using System;
using System.Threading.Tasks;

namespace IT.PubSub;

public interface IAsyncPublisher
{
    Task<Int64> PublishAsync(String channel, String message);

    Task<Int64> PublishAsync(String channel, Int64 message);

    Task<Int64> PublishAsync(String channel, UInt64 message);

    Task<Int64> PublishAsync(String channel, Double message);

    Task<Int64> PublishAsync(String channel, Byte[] message);

    Task<Int64> PublishAsync(String channel, ReadOnlyMemory<Byte> message);
}