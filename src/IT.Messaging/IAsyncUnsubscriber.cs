using System;
using System.Threading.Tasks;

namespace IT.Messaging;

public interface IAsyncUnsubscriber
{
    Task UnsubscribeAllAsync();

    Task UnsubscribeAsync(String channel);
}