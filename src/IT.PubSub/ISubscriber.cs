using System;

namespace IT.PubSub;

public interface ISubscriber : IAsyncSubscriber
{
    void UnsubscribeAll();

    void Unsubscribe(String channel);

    void Subscribe(String channel, Action<String, String> handler);

    void Subscribe(String channel, Action<String, Int64> handler);

    void Subscribe(String channel, Action<String, UInt64> handler);

    void Subscribe(String channel, Action<String, Double> handler);

    void Subscribe(String channel, Action<String, Byte[]> handler);

    void Subscribe(String channel, Action<String, ReadOnlyMemory<Byte>> handler);
}