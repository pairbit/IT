namespace IT.Working;

public interface IAsyncJobService : IAsyncJobScheduler, IAsyncJobDelayer, IAsyncJobAwaiter, IAsyncJobEnqueuer, IAsyncJobInformer
{
    
}