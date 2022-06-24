using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace IT.PubSub.Redis;

public class PubSub : IPubSub
{
    private readonly StackExchange.Redis.ISubscriber _subscriber;
    private readonly Boolean _isConcurrently;

    public PubSub(
        IConnectionMultiplexer multiplexer,
        Func<Options>? getOptions = null)
    {
        var options = getOptions?.Invoke();

        _isConcurrently = options is null || options.SubscriptionPolicy == SubscriptionPolicy.Concurrently;

        _subscriber = multiplexer.GetSubscriber();
    }

    #region IAsyncPublisher

    public Task<Int64> PublishAsync(String channel, String message) => _subscriber.PublishAsync(channel, message);

    public Task<Int64> PublishAsync(String channel, Int64 message) => _subscriber.PublishAsync(channel, message);

    public Task<Int64> PublishAsync(String channel, UInt64 message) => _subscriber.PublishAsync(channel, message);

    public Task<Int64> PublishAsync(String channel, Double message) => _subscriber.PublishAsync(channel, message);

    public Task<Int64> PublishAsync(String channel, Byte[] message) => _subscriber.PublishAsync(channel, message);

    public Task<Int64> PublishAsync(String channel, ReadOnlyMemory<Byte> message) => _subscriber.PublishAsync(channel, message);

    #endregion IAsyncPublisher

    #region IAsyncSubscriber

    public Boolean IsConnected(String? channel = null) => _subscriber.IsConnected(channel is not null ? (RedisChannel)channel : default);

    public Task UnsubscribeAllAsync() => _subscriber.UnsubscribeAllAsync();

    public Task UnsubscribeAsync(String channel) => _subscriber.UnsubscribeAsync(channel);

    public async Task SubscribeAsync(String channel, Action<String, String> handler)
    {
        if (_isConcurrently)
        {
            await _subscriber.SubscribeAsync(channel, (rChannel, rValue) => handler(rChannel, rValue)).ConfigureAwait(false);
        }
        else
        {
            (await _subscriber.SubscribeAsync(channel).ConfigureAwait(false)).OnMessage(message => handler(message.Channel, message.Message));
        }
    }

    public async Task SubscribeAsync(String channel, Action<String, Int64> handler)
    {
        if (_isConcurrently)
        {
            await _subscriber.SubscribeAsync(channel, (rChannel, rValue) => handler(rChannel, (Int64)rValue)).ConfigureAwait(false);
        }
        else
        {
            (await _subscriber.SubscribeAsync(channel).ConfigureAwait(false)).OnMessage(message => handler(message.Channel, (Int64)message.Message));
        }
    }

    public async Task SubscribeAsync(String channel, Action<String, UInt64> handler)
    {
        if (_isConcurrently)
        {
            await _subscriber.SubscribeAsync(channel, (rChannel, rValue) => handler(rChannel, (UInt64)rValue)).ConfigureAwait(false);
        }
        else
        {
            (await _subscriber.SubscribeAsync(channel).ConfigureAwait(false)).OnMessage(message => handler(message.Channel, (UInt64)message.Message));
        }
    }

    public async Task SubscribeAsync(String channel, Action<String, Double> handler)
    {
        if (_isConcurrently)
        {
            await _subscriber.SubscribeAsync(channel, (rChannel, rValue) => handler(rChannel, (Double)rValue)).ConfigureAwait(false);
        }
        else
        {
            (await _subscriber.SubscribeAsync(channel).ConfigureAwait(false)).OnMessage(message => handler(message.Channel, (Double)message.Message));
        }
    }

    public async Task SubscribeAsync(String channel, Action<String, Byte[]> handler)
    {
        if (_isConcurrently)
        {
            await _subscriber.SubscribeAsync(channel, (rChannel, rValue) => handler(rChannel, rValue)).ConfigureAwait(false);
        }
        else
        {
            (await _subscriber.SubscribeAsync(channel).ConfigureAwait(false)).OnMessage(message => handler(message.Channel, message.Message));
        }
    }

    public async Task SubscribeAsync(String channel, Action<String, ReadOnlyMemory<Byte>> handler)
    {
        if (_isConcurrently)
        {
            await _subscriber.SubscribeAsync(channel, (rChannel, rValue) => handler(rChannel, rValue)).ConfigureAwait(false);
        }
        else
        {
            (await _subscriber.SubscribeAsync(channel).ConfigureAwait(false)).OnMessage(message => handler(message.Channel, message.Message));
        }
    }

    #endregion IAsyncSubscriber

    #region IPublisher

    public Int64 Publish(String channel, String message) => _subscriber.Publish(channel, message);

    public Int64 Publish(String channel, Int64 message) => _subscriber.Publish(channel, message);

    public Int64 Publish(String channel, UInt64 message) => _subscriber.Publish(channel, message);

    public Int64 Publish(String channel, Double message) => _subscriber.Publish(channel, message);

    public Int64 Publish(String channel, Byte[] message) => _subscriber.Publish(channel, message);

    public Int64 Publish(String channel, ReadOnlyMemory<Byte> message) => _subscriber.Publish(channel, message);

    #endregion IPublisher

    #region ISubscriber

    public void UnsubscribeAll() => _subscriber.UnsubscribeAll();

    public void Unsubscribe(String channel) => _subscriber.Unsubscribe(channel);

    public void Subscribe(String channel, Action<String, String> handler)
    {
        if (_isConcurrently)
        {
            _subscriber.Subscribe(channel, (rChannel, rValue) => handler(rChannel, rValue));
        }
        else
        {
            _subscriber.Subscribe(channel).OnMessage(message => handler(message.Channel, message.Message));
        }
    }

    public void Subscribe(String channel, Action<String, Int64> handler)
    {
        if (_isConcurrently)
        {
            _subscriber.Subscribe(channel, (rChannel, rValue) => handler(rChannel, (Int64)rValue));
        }
        else
        {
            _subscriber.Subscribe(channel).OnMessage(message => handler(message.Channel, (Int64)message.Message));
        }
    }

    public void Subscribe(String channel, Action<String, UInt64> handler)
    {
        if (_isConcurrently)
        {
            _subscriber.Subscribe(channel, (rChannel, rValue) => handler(rChannel, (UInt64)rValue));
        }
        else
        {
            _subscriber.Subscribe(channel).OnMessage(message => handler(message.Channel, (UInt64)message.Message));
        }
    }

    public void Subscribe(String channel, Action<String, Double> handler)
    {
        if (_isConcurrently)
        {
            _subscriber.Subscribe(channel, (rChannel, rValue) => handler(rChannel, (Double)rValue));
        }
        else
        {
            _subscriber.Subscribe(channel).OnMessage(message => handler(message.Channel, (Double)message.Message));
        }
    }

    public void Subscribe(String channel, Action<String, Byte[]> handler)
    {
        if (_isConcurrently)
        {
            _subscriber.Subscribe(channel, (rChannel, rValue) => handler(rChannel, rValue));
        }
        else
        {
            _subscriber.Subscribe(channel).OnMessage(message => handler(message.Channel, message.Message));
        }
    }

    public void Subscribe(String channel, Action<String, ReadOnlyMemory<Byte>> handler)
    {
        if (_isConcurrently)
        {
            _subscriber.Subscribe(channel, (rChannel, rValue) => handler(rChannel, rValue));
        }
        else
        {
            _subscriber.Subscribe(channel).OnMessage(message => handler(message.Channel, message.Message));
        }
    }

    #endregion ISubscriber
}