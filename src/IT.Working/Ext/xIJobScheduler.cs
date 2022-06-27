using System;

namespace IT.Working;

public static class xIJobScheduler
{
    public static Boolean Schedule(this IJobScheduler jobScheduler, String schedule, Job job, Boolean update = true)
    {
        if (job is null) throw new ArgumentException(nameof(job));
        return jobScheduler.Schedule(schedule, job.Name, job.Arg, job.Queue, update);
    }

    public static Boolean ExistsSchedule(this IJobScheduler jobScheduler, Job job)
    {
        if (job is null) throw new ArgumentException(nameof(job));
        return jobScheduler.ExistsSchedule(job.Name, job.Arg, job.Queue);
    }

    public static String GetSchedule(this IJobScheduler jobScheduler, Job job)
    {
        if (job is null) throw new ArgumentException(nameof(job));
        return jobScheduler.GetSchedule(job.Name, job.Arg, job.Queue);
    }

    public static Boolean DeleteSchedule(this IJobScheduler jobScheduler, Job job)
    {
        if (job is null) throw new ArgumentException(nameof(job));
        return jobScheduler.DeleteSchedule(job.Name, job.Arg, job.Queue);
    }
}