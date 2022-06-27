using System.Threading.Tasks;

namespace IT.Working;

public interface IAsyncJobAwaiter
{
    //ContinueWith
    Task WaitAsync(Job parent, Job child);

    Task WaitAsync(params Job[] jobs);

    //ContinueWhenAll
    Task WaitAllAsync(Job[] parents, Job job);

    //ContinueWhenAny
    Task WaitAnyAsync(Job[] parents, Job job);
}