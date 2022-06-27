using System.Threading;
using System.Threading.Tasks;

namespace IT.Working;

public interface IAsyncWorker
{
    Task WorkAsync(CancellationToken cancellationToken);
}