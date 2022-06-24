using System;
using System.Threading.Tasks;

namespace IT.PubSub;

public interface IAsyncSubscriber
{
    Boolean IsConnected(String? channel = null);

    Task UnsubscribeAllAsync();

    Task UnsubscribeAsync(String channel);

    Task SubscribeAsync(String channel, Action<String, String> handler);

    Task SubscribeAsync(String channel, Action<String, Int64> handler);

    Task SubscribeAsync(String channel, Action<String, UInt64> handler);

    Task SubscribeAsync(String channel, Action<String, Double> handler);

    Task SubscribeAsync(String channel, Action<String, Byte[]> handler);

    Task SubscribeAsync(String channel, Action<String, ReadOnlyMemory<Byte>> handler);
}