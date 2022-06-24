namespace IT.PubSub.Redis;

//https://stackexchange.github.io/StackExchange.Redis/PubSubOrder.html
public enum SubscriptionPolicy
{
    Concurrently = 0,

    Sequentially
}