using IT.Data.Redis;
using IT.Data.Tests.Data;
using StackExchange.Redis;
using System.Text;

namespace IT.Data.Tests;

public class RedisDataTest : DataTest<Guid, City>
{
    public RedisDataTest() : base(NewCity())
    {

    }

    private static IDatabase GetDb()
    {
        var multi = ConnectionMultiplexer.Connect("redis1.sign:6379");
        return multi.GetDatabase(0);
    }

    private static HashRepository<Guid, string> NewString()
    {
        return new(GetDb(), "stringHashRepository", () => Guid.NewGuid());
    }

    private static HashRepository<Guid, City> NewCity()
    {
        return new(GetDb(), "cityHashRepository", () => Guid.NewGuid(),
            valueSerialize: CitySerialize, valueDeserialize: CityDeserialize);
    }

    private static Byte[] CitySerialize(City city)
    {
        var text = $"{city.Name}|{city.Count}";
        return Encoding.UTF8.GetBytes(text);
    }

    private static City CityDeserialize(ReadOnlyMemory<Byte> memory)
    {
        var span = memory.Span;

        var charCount = Encoding.UTF8.GetCharCount(span);

        var chars = new Char[charCount].AsSpan();

        Encoding.UTF8.GetChars(span, chars);

        var index = chars.IndexOf('|');

        if (index == -1) throw new FormatException();

        var name = chars.Slice(0, index).ToString();

        var count = Int32.Parse(chars.Slice(index + 1));

        return new City { Name = name, Count = count };
    }
}