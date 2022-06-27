using System;
using System.Threading.Tasks;

namespace IT.Working;

public static class xIAsyncJobDelayer
{
    public static Task<Boolean> DelayAsync(this IAsyncJobDelayer jobDelayer, Int64 ticks, Job job, Boolean repeat = false, Boolean? exists = null)
    {
        if (job is null) throw new ArgumentException(nameof(job));
        return jobDelayer.DelayAsync(ticks, job.Name, job.Arg, job.Queue, repeat, exists);
    }

    public static Task<Boolean> DelayAsync(this IAsyncJobDelayer jobDelayer, TimeSpan timeSpan, Job job, Boolean repeat = false, Boolean? exists = null)
    {
        if (job is null) throw new ArgumentException(nameof(job));
        return jobDelayer.DelayAsync(DateTime.UtcNow.Ticks + timeSpan.Ticks, job.Name, job.Arg, job.Queue, repeat, exists);
    }

    public static Task<Boolean> DelayAsync(this IAsyncJobDelayer jobDelayer, DateTime dateTime, Job job, Boolean repeat = false, Boolean? exists = null)
    {
        if (job is null) throw new ArgumentException(nameof(job));
        return jobDelayer.DelayAsync(dateTime.Ticks, job.Name, job.Arg, job.Queue, repeat, exists);
    }

    public static Task<Boolean> DelayAsync(this IAsyncJobDelayer jobDelayer, TimeSpan timeSpan, String name, String? arg = null, String? queue = null, Boolean repeat = false, Boolean? exists = null)
        => jobDelayer.DelayAsync(DateTime.UtcNow.Ticks + timeSpan.Ticks, name, arg, queue, repeat, exists);

    public static Task<Boolean> DelayAsync(this IAsyncJobDelayer jobDelayer, DateTime dateTime, String name, String? arg = null, String? queue = null, Boolean repeat = false, Boolean? exists = null)
        => jobDelayer.DelayAsync(dateTime.Ticks, name, arg, queue, repeat, exists);

    public static async Task<DateTime?> GetDelayDateTimeAsync(this IAsyncJobDelayer jobDelayer, String name, String? arg = null, String? queue = null, Boolean repeat = false)
    {
        var ticks = await jobDelayer.GetDelayAsync(name, arg, queue, repeat).ConfigureAwait(false);
        return ticks == null ? null : new DateTime(ticks.Value);
    }

    public static async Task<DateTime?> GetDelayDateTimeAsync(this IAsyncJobDelayer jobDelayer, Job job, Boolean repeat = false)
    {
        if (job is null) throw new ArgumentException(nameof(job));
        var ticks = await jobDelayer.GetDelayAsync(job.Name, job.Arg, job.Queue, repeat).ConfigureAwait(false);
        return ticks == null ? null : new DateTime(ticks.Value);
    }
}