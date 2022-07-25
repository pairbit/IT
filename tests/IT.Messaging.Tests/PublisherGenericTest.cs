namespace IT.Messaging.Tests;

public class PublisherGenericTest
{
    private IChannel<Guid> _channel;

    [SetUp]
    public void Setup(IChannel<Guid> channel)
    {
        _channel = channel;
    }

    [Test]
    public void Test1()
    {
        var guid = Guid.NewGuid();

        _channel.Subscribe("POIB-*", (channel, guid) =>
        {
            Console.WriteLine($"[{channel}] -> {guid}");
        });

        _channel.Publish("POIB-hash", guid);

        Assert.Pass();
    }
}