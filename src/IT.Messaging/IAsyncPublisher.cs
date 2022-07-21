using System;
using System.Threading.Tasks;

namespace IT.Messaging;

public interface IAsyncPublisher
{
    Task PublishAsync<T>(String channel, T message);

    Task PublishAsync(String channel, ReadOnlyMemory<Byte> message);
}