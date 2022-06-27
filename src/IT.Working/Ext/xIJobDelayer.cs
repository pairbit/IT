using System;

namespace IT.Working;

public static class xIJobDelayer
{
    public static Boolean Delay(this IJobDelayer jobDelayer, Int64 ticks, Job job, Boolean repeat = false, Boolean? exists = null)
    {
        if (job is null) throw new ArgumentException(nameof(job));
        return jobDelayer.Delay(ticks, job.Name, job.Arg, job.Queue, repeat, exists);
    }

    public static Boolean Delay(this IJobDelayer jobDelayer, TimeSpan timeSpan, Job job, Boolean repeat = false, Boolean? exists = null)
    {
        if (job is null) throw new ArgumentException(nameof(job));
        return jobDelayer.Delay(DateTime.UtcNow.Ticks + timeSpan.Ticks, job.Name, job.Arg, job.Queue, repeat, exists);
    }

    public static Boolean Delay(this IJobDelayer jobDelayer, DateTime dateTime, Job job, Boolean repeat = false, Boolean? exists = null)
    {
        if (job is null) throw new ArgumentException(nameof(job));
        return jobDelayer.Delay(dateTime.Ticks, job.Name, job.Arg, job.Queue, repeat, exists);
    }

    public static Boolean Delay(this IJobDelayer jobDelayer, TimeSpan timeSpan, String name, String? arg = null, String? queue = null, Boolean repeat = false, Boolean? exists = null)
        => jobDelayer.Delay(DateTime.UtcNow.Ticks + timeSpan.Ticks, name, arg, queue, repeat, exists);

    public static Boolean Delay(this IJobDelayer jobDelayer, DateTime dateTime, String name, String? arg = null, String? queue = null, Boolean repeat = false, Boolean? exists = null)
        => jobDelayer.Delay(dateTime.Ticks, name, arg, queue, repeat, exists);

    public static DateTime? GetDelayDateTime(this IJobDelayer jobDelayer, String name, String? arg = null, String? queue = null, Boolean repeat = false)
    {
        var ticks = jobDelayer.GetDelay(name, arg, queue, repeat);
        return ticks == null ? null : new DateTime(ticks.Value);
    }

    public static DateTime? GetDelayDateTime(this IJobDelayer jobDelayer, Job job, Boolean repeat = false)
    {
        if (job is null) throw new ArgumentException(nameof(job));
        var ticks = jobDelayer.GetDelay(job.Name, job.Arg, job.Queue, repeat);
        return ticks == null ? null : new DateTime(ticks.Value);
    }
}