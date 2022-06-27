using System;

namespace IT.Working;

public static class xIJobEnqueuer
{
    public static Boolean Enqueue(this IJobEnqueuer jobEnqueuer, Job job, Boolean repeat = false)
    {
        if (job is null) throw new ArgumentException(nameof(job));
        return jobEnqueuer.Enqueue(job.Name, job.Arg, job.Queue, repeat);
    }

    //public static void Enqueue<TType>(this IJobService jobService, String methodName, String arg = null, String queue = null)
    //    => jobService.Enqueue($"{typeof(TType).FullName}.{methodName}", arg, queue);
}