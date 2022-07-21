using System;

namespace IT.Messaging;

public interface ISubscriber<T> : IAsyncSubscriber<T>, IUnsubscriber
{
    void Subscribe(String channel, Action<String, T> handler);
}