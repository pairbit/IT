namespace IT.Messaging.Redis.PubSub;

public record Options
{
    public SubscriptionPolicy SubscriptionPolicy { get; set; }
}