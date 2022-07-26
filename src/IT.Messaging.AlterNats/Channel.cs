//using AlterNats;
//using System;
//using System.Threading.Tasks;

//namespace IT.Messaging.AlterNats;

//public class Channel : IChannel
//{
//    private readonly NatsConnection _connection;

//    public Channel(
//        NatsConnection connection)
//    {
//        _connection = connection;
//    }

//    #region IAsyncPublisher

//    public async Task PublishAsync<T>(String channel, T message) => await _connection.PublishAsync(channel, message).AsTask();

//    public async Task PublishAsync(String channel, ReadOnlyMemory<Byte> message) => await _connection.PublishAsync(channel, message).AsTask();

//    #endregion IAsyncPublisher

//    #region IPublisher

//    public void Publish<T>(String channel, T message) => _connection.PostPublish(channel, message);

//    public void Publish(String channel, ReadOnlyMemory<Byte> message) => _connection.PostPublish(channel, message);

//    #endregion IPublisher

//    #region IAsyncSubscriber

//    public Boolean IsConnected(String? channel = null) => _subscriber.IsConnected(channel is not null ? (RedisChannel)channel : default);

//    public async Task SubscribeAsync<T>(String channel, Action<String, T> handler)
//    {
//        if (_isConcurrently)
//        {
//            await _subscriber.SubscribeAsync(channel, (rChannel, rValue) => handler(rChannel, rValue)).ConfigureAwait(false);
//        }
//        else
//        {
//            (await _subscriber.SubscribeAsync(channel).ConfigureAwait(false)).OnMessage(message => handler(message.Channel, message.Message));
//        }
//    }

//    public async Task SubscribeAsync(String channel, Action<String, ReadOnlyMemory<Byte>> handler)
//    {
//        if (_isConcurrently)
//        {
//            await _subscriber.SubscribeAsync(channel, (rChannel, rValue) => handler(rChannel, rValue)).ConfigureAwait(false);
//        }
//        else
//        {
//            (await _subscriber.SubscribeAsync(channel).ConfigureAwait(false)).OnMessage(message => handler(message.Channel, message.Message));
//        }
//    }

//    #endregion IAsyncSubscriber

//    #region ISubscriber

//    public void Subscribe<T>(String channel, Action<String, T> handler)
//    {
//        if (_isConcurrently)
//        {
//            _subscriber.Subscribe(channel, (rChannel, rValue) => handler(rChannel, rValue));
//        }
//        else
//        {
//            _subscriber.Subscribe(channel).OnMessage(message => handler(message.Channel, message.Message));
//        }
//    }

//    public void Subscribe(String channel, Action<String, ReadOnlyMemory<Byte>> handler)
//    {
//        if (_isConcurrently)
//        {
//            _subscriber.Subscribe(channel, (rChannel, rValue) => handler(rChannel, rValue));
//        }
//        else
//        {
//            _subscriber.Subscribe(channel).OnMessage(message => handler(message.Channel, message.Message));
//        }
//    }

//    #endregion ISubscriber

//    #region IAsyncUnsubscriber

//    public Task UnsubscribeAllAsync() => _subscriber.UnsubscribeAllAsync();

//    public Task UnsubscribeAsync(String channel) => _subscriber.UnsubscribeAsync(channel);

//    #endregion IAsyncUnsubscriber

//    #region IUnsubscriber

//    public void UnsubscribeAll() => _subscriber.UnsubscribeAll();

//    public void Unsubscribe(String channel) => _subscriber.Unsubscribe(channel);

//    #endregion IUnsubscriber
//}