using System;

namespace IT.Messaging;

public interface IAsyncChannel : IAsyncPublisher, IAsyncSubscriber
{
    //Boolean IsConnected(String? channel = null);
}