using IT.Data.Redis;
using StackExchange.Redis;

namespace IT.Tests.Data;

public class RedisDataTest : DataTest<Guid, string>
{
    public RedisDataTest() : base(New())
    {

    }

    private static HashRepository<Guid, string> New()
    {
        var multi = ConnectionMultiplexer.Connect("redis1.sign:6379");
        var db = multi.GetDatabase(0);
        return new(db, "testHashRepository", () => Guid.NewGuid());
    }
}