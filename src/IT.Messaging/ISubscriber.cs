using System;

namespace IT.Messaging;

public interface ISubscriber : IAsyncSubscriber, IUnsubscriber
{
    void Subscribe<T>(String channel, Action<String, T> handler);

    void Subscribe(String channel, Action<String, ReadOnlyMemory<Byte>> handler);
}