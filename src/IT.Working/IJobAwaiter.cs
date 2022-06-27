namespace IT.Working;

public interface IJobAwaiter : IAsyncJobAwaiter
{
    void Wait(Job parent, Job child);

    void Wait(params Job[] jobs);

    void WaitAll(Job[] parents, Job job);

    void WaitAny(Job[] parents, Job job);
}