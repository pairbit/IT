using System;
using System.Threading.Tasks;

namespace IT.Messaging;

public interface IAsyncPublisher<T>
{
    Task PublishAsync(String channel, T message);
}