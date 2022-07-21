using System;
using System.Threading.Tasks;

namespace IT.Messaging;

public interface IAsyncSubscriber : IAsyncUnsubscriber
{
    Task SubscribeAsync<T>(String channel, Action<String, T> handler);

    Task SubscribeAsync(String channel, Action<String, ReadOnlyMemory<Byte>> handler);
}