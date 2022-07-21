using System;
using System.Threading.Tasks;

namespace IT.Messaging;

public interface IAsyncSubscriber<T> : IAsyncUnsubscriber
{
    Task SubscribeAsync(String channel, Action<String, T> handler);
}