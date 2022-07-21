using System;

namespace IT.Messaging;

public interface IUnsubscriber
{
    void UnsubscribeAll();

    void Unsubscribe(String channel);
}