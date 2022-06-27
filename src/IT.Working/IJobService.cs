namespace IT.Working;

public interface IJobService : IAsyncJobService, IJobScheduler, IJobDelayer, IJobAwaiter, IJobEnqueuer, IJobInformer
{
    
}