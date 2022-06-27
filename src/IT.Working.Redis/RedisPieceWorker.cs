using System;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Working.Redis;

/// <summary>
/// Piece worker
/// </summary>
public class RedisPieceWorker : IAsyncWorker
{
    public Task WorkAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}