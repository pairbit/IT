using IT.Working.Redis.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IT.Working.Redis;

public class RedisJobService : IJobService
{
    private const String _SharedQueueName = "Shared";
    private static readonly Int64 TicksOffset = new DateTime(2022, 06, 26).Ticks;

    private readonly IDatabase _db;
    private readonly Func<RedisWorkingOptions?>? _options;
    private readonly Func<RedisJobOptions?>? _jobOptions;

    public RedisJobService(IDatabase db,
        Func<RedisWorkingOptions?>? options = null,
        Func<RedisJobOptions?>? jobOptions = null)
    {
        _db = db;
        _options = options;
        _jobOptions = jobOptions;
    }

    #region IAsyncJobScheduler

    public Task<Boolean> ScheduleAsync(String schedule, String name, String? arg, String? queue, Boolean update)
    {
        if (schedule is null) throw new ArgumentNullException(nameof(schedule));
        if (schedule.Length == 0) throw new ArgumentException("is empty", nameof(schedule));

        return _db.HashSetAsync(GetQueueName("Scheduled"), JobToRedisValue(name, arg, queue), schedule, update ? When.Always : When.NotExists);
    }

    public Task ScheduleAsync(params JobSchedule[] schedules)
    {
        if (schedules is null) throw new ArgumentNullException(nameof(schedules));
        if (schedules.Length == 0) throw new ArgumentException("is empty", nameof(schedules));
        //Arg.Unique(schedules, x => x.Job);
        return _db.HashSetAsync(GetQueueName("Scheduled"), schedules.To(JobScheduleToHashEntry));
    }

    public Task<Boolean> ExistsScheduleAsync(String name, String? arg = null, String? queue = null)
        => _db.HashExistsAsync(GetQueueName("Scheduled"), JobToRedisValue(name, arg, queue));

    public Task<Int64> GetCountSchedulesAsync() => _db.HashLengthAsync(GetQueueName("Scheduled"));

    public async Task<String> GetScheduleAsync(String name, String? arg, String? queue)
        => await _db.HashGetAsync(GetQueueName("Scheduled"), JobToRedisValue(name, arg, queue)).ConfigureAwait(false);

    public async Task<String[]> GetSchedulesAsync(params Job[] jobs)
        => (await _db.HashGetAsync(GetQueueName("Scheduled"), jobs.To(JobToRedisValue)).ConfigureAwait(false)).To(x => (String)x);

    public async Task<JobSchedule[]> GetAllSchedulesAsync()
        => (await _db.HashGetAllAsync(GetQueueName("Scheduled")).ConfigureAwait(false)).To(HashEntryToJobSchedule);

    public Task<Boolean> DeleteScheduleAsync(String name, String? arg, String? queue)
        => _db.HashDeleteAsync(GetQueueName("Scheduled"), JobToRedisValue(name, arg, queue));

    public Task<Int64> DeleteSchedulesAsync(params Job[] jobs)
        => _db.HashDeleteAsync(GetQueueName("Scheduled"), jobs.To(JobToRedisValue));

    public Task<Boolean> DeleteAllSchedulesAsync()
        => _db.KeyDeleteAsync(GetQueueName("Scheduled"));

    #endregion IAsyncJobScheduler

    #region IJobScheduler

    public Boolean Schedule(String schedule, String name, String? arg, String? queue, Boolean update)
    {
        if (schedule is null) throw new ArgumentNullException(nameof(schedule));
        if (schedule.Length == 0) throw new ArgumentException("is empty", nameof(schedule));

        return _db.HashSet(GetQueueName("Scheduled"), JobToRedisValue(name, arg, queue), schedule, update ? When.Always : When.NotExists);
    }

    public void Schedule(params JobSchedule[] schedules)
    {
        if (schedules is null) throw new ArgumentNullException(nameof(schedules));
        if (schedules.Length == 0) throw new ArgumentException("is empty", nameof(schedules));
        //Arg.Unique(schedules, x => x.Job);
        _db.HashSet(GetQueueName("Scheduled"), schedules.To(JobScheduleToHashEntry));
    }

    public Boolean ExistsSchedule(String name, String? arg = null, String? queue = null)
        => _db.HashExists(GetQueueName("Scheduled"), JobToRedisValue(name, arg, queue));

    public Int64 GetCountSchedules() => _db.HashLength(GetQueueName("Scheduled"));

    public String GetSchedule(String name, String? arg, String? queue)
        => _db.HashGet(GetQueueName("Scheduled"), JobToRedisValue(name, arg, queue));

    public String[] GetSchedules(params Job[] jobs)
        => _db.HashGet(GetQueueName("Scheduled"), jobs.To(JobToRedisValue)).To(x => (String)x);

    public JobSchedule[] GetAllSchedules()
        => _db.HashGetAll(GetQueueName("Scheduled")).To(HashEntryToJobSchedule);

    public Boolean DeleteSchedule(String name, String? arg, String? queue)
        => _db.HashDelete(GetQueueName("Scheduled"), JobToRedisValue(name, arg, queue));

    public Int64 DeleteSchedules(params Job[] jobs)
        => _db.HashDelete(GetQueueName("Scheduled"), jobs.To(JobToRedisValue));

    public Boolean DeleteAllSchedules()
        => _db.KeyDelete(GetQueueName("Scheduled"));

    #endregion IJobScheduler

    #region IAsyncJobDelayer

    public Task<Boolean> DelayAsync(Int64 ticks, String name, String? arg, String? queue, Boolean repeat, Boolean? exists)
    {
        //Arg.GreaterThan(ticks, DateTime.UtcNow.Ticks);

        ticks = ticks > TicksOffset ? ticks - TicksOffset : 0;

        return _db.SortedSetAddAsync(GetQueueName("Delayed"), DelayToRedisValue(name, arg, queue, repeat), ticks, ToWhen(exists));
    }

    public Task<Int64> GetCountDelaysAsync() => _db.SortedSetLengthAsync(GetQueueName("Delayed"), exclude: Exclude.Both);

    public async Task<Int64?> GetDelayAsync(String name, String? arg, String? queue, Boolean repeat)
    {
        var delay = (Int64?)await _db.SortedSetScoreAsync(GetQueueName("Delayed"), DelayToRedisValue(name, arg, queue, repeat)).ConfigureAwait(false);

        if (delay == null) return null;

        var value = delay.Value;

        return value > 0 ? value + TicksOffset : value;
    }

    public async Task<JobDelay[]> GetDelaysByRangeAsync(Int64 minTicks, Int64 maxTicks, Boolean withTicks = false, Boolean ascending = true, Int64 skip = 0, Int64 take = -1)
    {
        var key = GetQueueName("Delayed");
        minTicks = minTicks > TicksOffset ? minTicks - TicksOffset : 0;
        maxTicks = maxTicks > TicksOffset ? maxTicks - TicksOffset : 0;
        var exclude = Exclude.None;
        var order = ascending ? Order.Ascending : Order.Descending;

        return withTicks ? (await _db.SortedSetRangeByScoreWithScoresAsync(key, minTicks, maxTicks, exclude, order, skip, take).ConfigureAwait(false)).To(JobDelayFromRedis)
                         : (await _db.SortedSetRangeByScoreAsync(key, minTicks, maxTicks, exclude, order, skip, take).ConfigureAwait(false)).To(JobDelayFromRedis);
    }

    public Task<Boolean> DeleteDelayAsync(String name, String? arg, String? queue, Boolean repeat)
        => _db.SortedSetRemoveAsync(GetQueueName("Delayed"), DelayToRedisValue(name, arg, queue, repeat));

    public Task<Boolean> DeleteAllDelaysAsync() => _db.KeyDeleteAsync(GetQueueName("Delayed"));

    #endregion IAsyncJobDelayer

    #region IJobDelayer

    public Boolean Delay(Int64 ticks, String name, String? arg, String? queue, Boolean repeat, Boolean? exists)
    {
        //Arg.GreaterThan(ticks, DateTime.UtcNow.Ticks);

        ticks = ticks > TicksOffset ? ticks - TicksOffset : 0;

        return _db.SortedSetAdd(GetQueueName("Delayed"), DelayToRedisValue(name, arg, queue, repeat), ticks, ToWhen(exists));
    }

    public Int64 GetCountDelays() => _db.SortedSetLength(GetQueueName("Delayed"), exclude: Exclude.Both);

    public Int64? GetDelay(String name, String? arg, String? queue, Boolean repeat)
    {
        var delay = (Int64?)_db.SortedSetScore(GetQueueName("Delayed"), DelayToRedisValue(name, arg, queue, repeat));

        if (delay == null) return null;

        var value = delay.Value;

        return value > 0 ? value + TicksOffset : value;
    }

    public JobDelay[] GetDelaysByRange(Int64 minTicks, Int64 maxTicks, Boolean withTicks = false, Boolean ascending = true, Int64 skip = 0, Int64 take = -1)
    {
        var key = GetQueueName("Delayed");
        minTicks = minTicks > TicksOffset ? minTicks - TicksOffset : 0;
        maxTicks = maxTicks > TicksOffset ? maxTicks - TicksOffset : 0;
        var exclude = Exclude.None;
        var order = ascending ? Order.Ascending : Order.Descending;

        return withTicks ? _db.SortedSetRangeByScoreWithScores(key, minTicks, maxTicks, exclude, order, skip, take).To(JobDelayFromRedis)
                         : _db.SortedSetRangeByScore(key, minTicks, maxTicks, exclude, order, skip, take).To(JobDelayFromRedis);
    }

    public Boolean DeleteDelay(String name, String? arg, String? queue, Boolean repeat)
        => _db.SortedSetRemove(GetQueueName("Delayed"), DelayToRedisValue(name, arg, queue, repeat));

    public Boolean DeleteAllDelays() => _db.KeyDelete(GetQueueName("Delayed"));

    #endregion IJobDelayer

    #region IAsyncJobAwaiter

    public Task WaitAsync(Job parent, Job child)
    {
        throw new NotImplementedException();
    }

    public Task WaitAsync(params Job[] jobs)
    {
        //Arg.NotNullEmpty(jobs);
        //Arg.MinLength(jobs, 2);

        throw new NotImplementedException();
    }

    public Task WaitAllAsync(Job[] parents, Job job)
    {
        throw new NotImplementedException();
    }

    public Task WaitAnyAsync(Job[] parents, Job job)
    {
        throw new NotImplementedException();
    }

    #endregion IAsyncJobAwaiter

    #region IJobAwaiter

    public void Wait(Job parent, Job child)
    {
        throw new NotImplementedException();
    }

    public void Wait(params Job[] jobs)
    {
        //Arg.NotNullEmpty(jobs);
        //Arg.MinLength(jobs, 2);

        throw new NotImplementedException();
    }

    public void WaitAll(Job[] parents, Job job)
    {
        throw new NotImplementedException();
    }

    public void WaitAny(Job[] parents, Job job)
    {
        throw new NotImplementedException();
    }

    #endregion IJobAwaiter

    #region IAsyncJobEnqueuer

    public async Task<Boolean> EnqueueAsync(String name, String? arg, String? queue, Boolean repeat)
    {
        var value = ConvertToRedisValue(name, arg, ref queue);

        if (repeat)
        {
            var status = GetJobStatus(await _db.HashGetAsync($"{queue}:Status", value).ConfigureAwait(false));

            if (status == null || status > JobStatus.Processing)
            {
                await _db.SetStatusAsync(queue!, value, JobStatus.Enqueued).ConfigureAwait(false);
                await _db.ListLeftPushAsync($"{queue}:Enqueued", value).ConfigureAwait(false);
                return true;
            }
        }
        else
        {
            if (await _db.SetStatusAsync(queue!, value, JobStatus.Enqueued, When.NotExists).ConfigureAwait(false))
            {
                await _db.ListLeftPushAsync($"{queue}:Enqueued", value).ConfigureAwait(false);
                return true;
            }
        }
        return false;
    }

    public async Task<Boolean[]> EnqueueAsync(params JobRepeat[] jobs)
    {
        if (jobs is null) throw new ArgumentNullException(nameof(jobs));
        if (jobs.Length == 0) throw new ArgumentException("is empty", nameof(jobs));

        var result = new Boolean[jobs.Length];

        for (int i = 0; i < jobs.Length; i++)
        {
            var jobRepeat = jobs[i];
            var job = jobRepeat.Job;
            result[i] = await EnqueueAsync(job.Name, job.Arg, job.Queue, jobRepeat.Repeat).ConfigureAwait(false);
        }

        return result;
    }

    #endregion IAsyncJobEnqueuer

    #region IJobEnqueuer

    public Boolean Enqueue(String name, String? arg, String? queue, Boolean repeat)
    {
        var value = ConvertToRedisValue(name, arg, ref queue);

        if (repeat)
        {
            var status = GetJobStatus(_db.HashGet($"{queue}:Status", value));

            if (status == null || status > JobStatus.Processing)
            {
                _db.SetStatus(queue!, value, JobStatus.Enqueued);
                _db.ListLeftPush($"{queue}:Enqueued", value);
                return true;
            }
        }
        else
        {
            if (_db.SetStatus(queue!, value, JobStatus.Enqueued, When.NotExists))
            {
                _db.ListLeftPush($"{queue}:Enqueued", value);
                return true;
            }
        }
        return false;
    }

    public Boolean[] Enqueue(params JobRepeat[] jobs)
    {
        if (jobs is null) throw new ArgumentNullException(nameof(jobs));
        if (jobs.Length == 0) throw new ArgumentException("is empty", nameof(jobs));

        var result = new Boolean[jobs.Length];

        for (int i = 0; i < jobs.Length; i++)
        {
            var jobRepeat = jobs[i];
            var job = jobRepeat.Job;
            result[i] = Enqueue(job.Name, job.Arg, job.Queue, jobRepeat.Repeat);
        }

        return result;
    }

    #endregion IJobEnqueuer

    #region IAsyncJobInformer

    public Task<Boolean> ExistsAsync(String name, String? arg, String? queue)
    {
        var value = ConvertToRedisValue(name, arg, ref queue);
        return _db.HashExistsAsync($"{queue}:Status", value);
    }

    public async Task<JobStatus?> GetLastStatusAsync(String name, String? arg, String? queue)
    {
        var value = ConvertToRedisValue(name, arg, ref queue);
        return GetJobStatus(await _db.HashGetAsync($"{queue}:Status", value).ConfigureAwait(false));
    }

    public Task<Boolean> DeleteStatusAsync(String name, String? arg, String? queue)
    {
        var value = ConvertToRedisValue(name, arg, ref queue);
        return _db.HashDeleteAsync($"{queue}:Status", value);
    }

    public async Task<Int64> DeleteStatusesAsync(params Job[] jobs)
    {
        if (jobs is null) throw new ArgumentNullException(nameof(jobs));
        if (jobs.Length == 0) throw new ArgumentException("is empty", nameof(jobs));

        Int64 deleted = 0;

        foreach (var group in GroupByQueue(jobs))
        {
            deleted += await _db.HashDeleteAsync($"{group.Key}:Status", group.Value.ToArray()).ConfigureAwait(false);
        }

        return deleted;
    }

    #endregion IAsyncJobInformer

    #region IJobInformer

    public Boolean Exists(String name, String? arg, String? queue)
    {
        var value = ConvertToRedisValue(name, arg, ref queue);
        return _db.HashExists($"{queue}:Status", value);
    }

    public JobStatus? GetLastStatus(String name, String? arg, String? queue)
    {
        var value = ConvertToRedisValue(name, arg, ref queue);
        return GetJobStatus(_db.HashGet($"{queue}:Status", value));
    }

    public Boolean DeleteStatus(String name, String? arg, String? queue)
    {
        var value = ConvertToRedisValue(name, arg, ref queue);
        return _db.HashDelete($"{queue}:Status", value);
    }

    public Int64 DeleteStatuses(params Job[] jobs)
    {
        if (jobs is null) throw new ArgumentNullException(nameof(jobs));
        if (jobs.Length == 0) throw new ArgumentException("is empty", nameof(jobs));

        Int64 deleted = 0;

        foreach (var group in GroupByQueue(jobs))
        {
            deleted += _db.HashDelete($"{group.Key}:Status", group.Value.ToArray());
        }

        return deleted;
    }

    #endregion IJobInformer

    #region Private

    private String GetQueueName(String queue)
    {
        var options = _options?.Invoke();
        var prefix = options?.Prefix;
        return prefix is null || prefix.Length == 0 ? $"IT.Working:Queues:{queue}" : $"{prefix}:{queue}";
    }

    private IReadOnlyDictionary<String, List<RedisValue>> GroupByQueue(Job[] jobs)
    {
        var byQueue = new Dictionary<String, List<RedisValue>>(jobs.Length);

        foreach (var job in jobs)
        {
            if (job is null) continue;

            var queue = job.Queue;

            var value = ConvertToRedisValue(job.Name, job.Arg, ref queue);

            if (byQueue.TryGetValue(queue!, out var values))
            {
                values.Add(value);
            }
            else
            {
                byQueue.Add(queue!, new List<RedisValue> { value });
            }
        }

        return byQueue;
    }

    private JobStatus? GetJobStatus(RedisValue value) => value.IsNull ? null : (JobStatus)(Int64)value;

    private When ToWhen(Boolean? exists) => exists != null ? exists == true ? When.Exists : When.NotExists : When.Always;

    private Job JobFromRedis(ReadOnlySpan<Char> value)
    {
        //{name}:{arg}:{queue}
        var index = value.IndexOf(':');

        if (index > -1)
        {
            var name = value.Slice(0, index).ToString();

            //{arg}:{queue}
            value = value.Slice(index + 1);

            index = value.IndexOf(':');

            if (index > -1)
            {
                var argSpan = value.Slice(0, index);

                var arg = argSpan.IsEmpty ? null : argSpan.ToString();

                //{queue}
                var queueSpan = value.Slice(index + 1);

                var queue = queueSpan.IsEmpty ? null : queueSpan.ToString();

                return new Job(name) { Arg = arg, Queue = queue };
            }
            else return new Job(name) { Arg = value.ToString() };
        }
        else return new Job(value.ToString());
    }

    private JobDelay JobDelayFromRedisWith(RedisValue value, Int64 delay)
    {
        var span = ((String)value).AsSpan();

        var repeat = span[0] == '1';

        var job = JobFromRedis(span.Slice(1));

        return new JobDelay { Job = job, Repeat = repeat, Delay = delay };
    }

    private JobDelay JobDelayFromRedis(RedisValue value) => JobDelayFromRedisWith(value, 0);

    private JobDelay JobDelayFromRedis(SortedSetEntry entry)
    {
        var delay = (Int64)entry.Score;

        if (delay > 0) delay += TicksOffset;

        return JobDelayFromRedisWith(entry.Element, delay);
    }

    private RedisValue DelayToRedisValue(String name, String? arg, String? queue, Boolean repeat)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("is empty", nameof(name));
        if (name.IndexOf(':') > -1) throw new ArgumentException("contains ':'", nameof(name));

        if (arg is not null)
        {
            if (arg.Length == 0) throw new ArgumentException("is empty", nameof(arg));
            if (arg.IndexOf(':') > -1) throw new ArgumentException("contains ':'", nameof(arg));
        }

        if (queue is not null)
        {
            if (queue.Length == 0) throw new ArgumentException("is empty", nameof(queue));
            if (queue.IndexOf(':') > -1) throw new ArgumentException("contains ':'", nameof(queue));
        }

        return $"{(repeat ? 1 : 0)}{name}:{arg}:{queue}";
    }

    private RedisValue JobToRedisValue(String name, String? arg, String? queue)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("is empty", nameof(name));
        if (name.IndexOf(':') > -1) throw new ArgumentException("contains ':'", nameof(name));

        if (arg is not null)
        {
            if (arg.Length == 0) throw new ArgumentException("is empty", nameof(arg));
            if (arg.IndexOf(':') > -1) throw new ArgumentException("contains ':'", nameof(arg));
        }

        if (queue is not null)
        {
            if (queue.Length == 0) throw new ArgumentException("is empty", nameof(queue));
            if (queue.IndexOf(':') > -1) throw new ArgumentException("contains ':'", nameof(queue));
        }

        return $"{name}:{arg}:{queue}";
    }

    private RedisValue JobToRedisValue(Job job) => JobToRedisValue(job.Name, job.Arg, job.Queue);

    private HashEntry JobScheduleToHashEntry(JobSchedule jobSchedule, Int32 index)
    {
        var job = jobSchedule.Job;

        if (job is null) throw new ArgumentNullException($"schedules[{index}].Job");

        var jobName = job.Name;
        if (jobName is null) throw new ArgumentNullException($"schedules[{index}].Job.Name");
        if (jobName.Length == 0) throw new ArgumentException("is empty", $"schedules[{index}].Job.Name");
        if (jobName.IndexOf(':') > -1) throw new ArgumentException("contains ':'", $"schedules[{index}].Job.Name");

        var jobArg = job.Arg;
        if (jobArg is not null)
        {
            if (jobArg.Length == 0) throw new ArgumentException("is empty", $"schedules[{index}].Job.Arg");
            if (jobArg.IndexOf(':') > -1) throw new ArgumentException("contains ':'", $"schedules[{index}].Job.Arg");
        }

        var jobQueue = job.Queue;
        if (jobQueue is not null)
        {
            if (jobQueue.Length == 0) throw new ArgumentException("is empty", $"schedules[{index}].Job.Queue");
            if (jobQueue.IndexOf(':') > -1) throw new ArgumentException("contains ':'", $"schedules[{index}].Job.Queue");
        }

        var schedule = jobSchedule.Schedule;
        if (schedule is null) throw new ArgumentNullException($"schedules[{index}].Schedule");
        if (schedule.Length == 0) throw new ArgumentException("is empty", $"schedules[{index}].Schedule");

        return new HashEntry($"{jobName}:{jobArg}:{jobQueue}", schedule);
    }

    private JobSchedule HashEntryToJobSchedule(HashEntry hashEntry) => new() { Job = JobFromRedis(((String)hashEntry.Name).AsSpan()), Schedule = hashEntry.Value };

    //private HashEntry JobDelayToHashEntry(JobDelay jobDelay, Int32 index)
    //{
    //    var item = $"[{index}].";
    //    Arg.NotNull(jobDelay.Job, paramName: item + nameof(jobDelay.Job));
    //    Arg.GreaterThan(jobDelay.Delay, DateTime.UtcNow.Ticks, item + nameof(jobDelay.Delay));
    //    return new HashEntry(JobToRedisValue(jobDelay.Job), jobDelay.Delay);
    //}

    private RedisValue ConvertToRedisValue(String name, String? arg, ref String? queue)
    {
        if (name is null) throw new ArgumentNullException(nameof(name));
        if (name.Length == 0) throw new ArgumentException("is empty", nameof(name));
        if (name.IndexOf(':') > -1) throw new ArgumentException("contains ':'", nameof(name));

        if (queue is null)
        {
            var jobOptions = _jobOptions?.Invoke();
            if (jobOptions is null)
            {
                queue = _SharedQueueName;
            }
            else
            {
                if (jobOptions.QueuePolicy == QueuePolicy.Individual)
                {
                    queue = GetQueueName(name);
                    if (arg is not null)
                    {
                        if (arg.Length == 0) throw new ArgumentException("is empty", nameof(arg));
                        if (arg.IndexOf(':') > -1) throw new ArgumentException("contains ':'", nameof(arg));
                        return arg;
                    }
                    return RedisValue.EmptyString;
                }
                else
                {
                    queue = jobOptions.SharedQueueName ?? _SharedQueueName;
                }
            }
        }
        else
        {
            if (queue.Length == 0) throw new ArgumentException("is empty", nameof(queue));
            if (queue.IndexOf(':') > -1) throw new ArgumentException("contains ':'", nameof(queue));
        }

        queue = GetQueueName(queue);

        if (arg is not null)
        {
            if (arg.Length == 0) throw new ArgumentException("is empty", nameof(arg));
            if (arg.IndexOf(':') > -1) throw new ArgumentException("contains ':'", nameof(arg));
            return $"{name}:{arg}";
        }
        return name;
    }

    #endregion Private
}