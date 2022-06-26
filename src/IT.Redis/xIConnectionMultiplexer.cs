using System.Threading.Tasks;

namespace StackExchange.Redis;

public static class xIConnectionMultiplexer
{
    public static void FlushAll(this IConnectionMultiplexer multiplexer,
        bool configuredOnly = false, CommandFlags flags = CommandFlags.None, object? asyncState = null)
    {
        foreach (var endpoint in multiplexer.GetEndPoints(configuredOnly))
        {
            var server = multiplexer.GetServer(endpoint, asyncState);

            server.FlushAllDatabases(flags);
        }
    }

    public static async Task FlushAllAsync(this IConnectionMultiplexer multiplexer,
        bool configuredOnly = false, CommandFlags flags = CommandFlags.None, object? asyncState = null)
    {
        foreach (var endpoint in multiplexer.GetEndPoints(configuredOnly))
        {
            var server = multiplexer.GetServer(endpoint, asyncState);

            await server.FlushAllDatabasesAsync(flags).ConfigureAwait(false);
        }
    }
}