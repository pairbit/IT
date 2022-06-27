using System;
using System.Threading.Tasks;

namespace IT.Working;

public static class xIAsyncJobScheduler
{
    public static Task<Boolean> ScheduleAsync(this IAsyncJobScheduler jobScheduler, String schedule, Job job, Boolean update = true)
    {
        if (job is null) throw new ArgumentException(nameof(job));
        return jobScheduler.ScheduleAsync(schedule, job.Name, job.Arg, job.Queue, update);
    }

    public static Task<Boolean> ExistsScheduleAsync(this IAsyncJobScheduler jobScheduler, Job job)
    {
        if (job is null) throw new ArgumentException(nameof(job));
        return jobScheduler.ExistsScheduleAsync(job.Name, job.Arg, job.Queue);
    }

    public static Task<String> GetScheduleAsync(this IAsyncJobScheduler jobScheduler, Job job)
    {
        if (job is null) throw new ArgumentException(nameof(job));
        return jobScheduler.GetScheduleAsync(job.Name, job.Arg, job.Queue);
    }

    public static Task<Boolean> DeleteScheduleAsync(this IAsyncJobScheduler jobScheduler, Job job)
    {
        if (job is null) throw new ArgumentException(nameof(job));
        return jobScheduler.DeleteScheduleAsync(job.Name, job.Arg, job.Queue);
    }
}