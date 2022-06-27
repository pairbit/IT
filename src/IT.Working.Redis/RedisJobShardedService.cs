//using IT.Design;
//using IT.Linq;
//using IT.Redis.Working.Options;
//using IT.Validation;
//using IT.Working;
//using Microsoft.Extensions.Options;
//using StackExchange.Redis;
//using System;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;

//namespace IT.Working.Redis;

//public class RedisJobShardedService : IJobService
//{
//    private static readonly Int64 TicksOffset = new DateTime(2022, 05, 10).Ticks;
//    private static readonly Encoding _encoding = Encoding.UTF8;

//    private readonly UInt64 _shards;
//    private readonly IDatabase[] _databases;
//    private readonly RedisWorkingOptions _options;
//    private readonly RedisJobOptions _clientOptions;

//    public RedisJobShardedService(
//        IOptionsSnapshot<RedisWorkingOptions> options,
//        IOptionsSnapshot<RedisJobOptions> clientOptions,
//        IFactory<IDatabase> databaseFactory)
//    {
//        _options = options.Value;
//        _clientOptions = clientOptions.Value;
//        var databases = _options.ConnectionString.Split(';').To(x => databaseFactory.GetRequired("ConnectionStrings:" + x));
//        if (databases.Length == 1) throw new NotSupportedException("for single queues use 'RedisJobService'");
//        _databases = databases;
//        _shards = (UInt64)_databases.Length;
//    }

//    #region IAsyncJobScheduler

//    public Task<Boolean> ScheduleAsync(String schedule, String name, String? arg, String? queue, Boolean update) => throw new NotImplementedException();
//    //{
//    //    Arg.NotNullEmptySpace(schedule);
//    //    return GetDatabase().HashSetAsync(_options.GetQueueName("Scheduled"), JobToRedisValue(name, arg, queue), schedule, update ? When.Always : When.NotExists);
//    //}

//    public Task ScheduleAsync(params JobSchedule[] schedules) => throw new NotImplementedException();
//    //{
//    //    Arg.NotNullEmpty(schedules);
//    //    Arg.Unique(schedules, x => x.Job);
//    //    return GetDatabase().HashSetAsync(_options.GetQueueName("Scheduled"), schedules.To(JobScheduleToHashEntry));
//    //}

//    public Task<Boolean> ExistsScheduleAsync(String name, String? arg = null, String? queue = null) => throw new NotImplementedException();
//    //=> GetDatabase().HashExistsAsync(_options.GetQueueName("Scheduled"), JobToRedisValue(name, arg, queue));

//    public Task<Int64> GetCountSchedulesAsync() => throw new NotImplementedException();
//    //=> GetDatabase().HashLengthAsync(_options.GetQueueName("Scheduled"));

//    public async Task<String> GetScheduleAsync(String name, String? arg, String? queue) => throw new NotImplementedException();
//    //=> await GetDatabase().HashGetAsync(_options.GetQueueName("Scheduled"), JobToRedisValue(name, arg, queue)).CA();

//    public async Task<String[]> GetSchedulesAsync(params Job[] jobs) => throw new NotImplementedException();
//    //=> (await GetDatabase().HashGetAsync(_options.GetQueueName("Scheduled"), jobs.To(JobToRedisValue)).CA()).To(x => (String)x);

//    public async Task<JobSchedule[]> GetAllSchedulesAsync() => throw new NotImplementedException();
//    //=> (await GetDatabase().HashGetAllAsync(_options.GetQueueName("Scheduled")).CA()).To(HashEntryToJobSchedule);

//    public Task<Boolean> DeleteScheduleAsync(String name, String? arg, String? queue) => throw new NotImplementedException();
//    //=> GetDatabase().HashDeleteAsync(_options.GetQueueName("Scheduled"), JobToRedisValue(name, arg, queue));

//    public Task<Int64> DeleteSchedulesAsync(params Job[] jobs) => throw new NotImplementedException();
//    //=> GetDatabase().HashDeleteAsync(_options.GetQueueName("Scheduled"), jobs.To(JobToRedisValue));

//    public Task<Boolean> DeleteAllSchedulesAsync() => throw new NotImplementedException();
//    //=> GetDatabase().KeyDeleteAsync(_options.GetQueueName("Scheduled"));

//    #endregion IAsyncJobScheduler

//    #region IJobScheduler

//    public Boolean Schedule(String schedule, String name, String? arg, String? queue, Boolean update) => throw new NotImplementedException();
//    //{
//    //    Arg.NotNullEmptySpace(schedule);
//    //    return GetDatabase().HashSet(_options.GetQueueName("Scheduled"), JobToRedisValue(name, arg, queue), schedule, update ? When.Always : When.NotExists);
//    //}

//    public void Schedule(params JobSchedule[] schedules) => throw new NotImplementedException();
//    //{
//    //    Arg.NotNullEmpty(schedules);
//    //    Arg.Unique(schedules, x => x.Job);
//    //    GetDatabase().HashSet(_options.GetQueueName("Scheduled"), schedules.To(JobScheduleToHashEntry));
//    //}

//    public Boolean ExistsSchedule(String name, String? arg = null, String? queue = null) => throw new NotImplementedException();
//    //=> GetDatabase().HashExists(_options.GetQueueName("Scheduled"), JobToRedisValue(name, arg, queue));

//    public Int64 GetCountSchedules() => throw new NotImplementedException();
//    //=> GetDatabase().HashLength(_options.GetQueueName("Scheduled"));

//    public String GetSchedule(String name, String? arg, String? queue) => throw new NotImplementedException();
//    //=> GetDatabase().HashGet(_options.GetQueueName("Scheduled"), JobToRedisValue(name, arg, queue));

//    public String[] GetSchedules(params Job[] jobs) => throw new NotImplementedException();
//    //=> GetDatabase().HashGet(_options.GetQueueName("Scheduled"), jobs.To(JobToRedisValue)).To(x => (String)x);

//    public JobSchedule[] GetAllSchedules() => throw new NotImplementedException();
//    //=> GetDatabase().HashGetAll(_options.GetQueueName("Scheduled")).To(HashEntryToJobSchedule);

//    public Boolean DeleteSchedule(String name, String? arg, String? queue) => throw new NotImplementedException();
//    //=> GetDatabase().HashDelete(_options.GetQueueName("Scheduled"), JobToRedisValue(name, arg, queue));

//    public Int64 DeleteSchedules(params Job[] jobs) => throw new NotImplementedException();
//    //=> GetDatabase().HashDelete(_options.GetQueueName("Scheduled"), jobs.To(JobToRedisValue));

//    public Boolean DeleteAllSchedules() => throw new NotImplementedException();
//    //=> GetDatabase().KeyDelete(_options.GetQueueName("Scheduled"));

//    #endregion IJobScheduler

//    #region IAsyncJobDelayer

//    public Task<Boolean> DelayAsync(Int64 ticks, String name, String? arg, String? queue, Boolean repeat, Boolean? exists)
//    {
//        //Arg.GreaterThan(ticks, DateTime.UtcNow.Ticks);

//        ticks = ticks > TicksOffset ? ticks - TicksOffset : 0;

//        var member = DelayToRedisValue(name, arg, queue, repeat, out var db);

//        return db.SortedSetAddAsync(_options.GetQueueName("Delayed"), member, ticks, ToWhen(exists));
//    }

//    public async Task<Int64> GetCountDelaysAsync()
//    {
//        Int64 count = 0;
//        foreach (var db in _databases)
//        {
//            count += await db.SortedSetLengthAsync(_options.GetQueueName("Delayed"), exclude: Exclude.Both).CA();
//        }
//        return count;
//    }

//    public async Task<Int64?> GetDelayAsync(String name, String? arg, String? queue, Boolean repeat)
//    {
//        var member = DelayToRedisValue(name, arg, queue, repeat, out var db);
//        var delay = (Int64?)await db.SortedSetScoreAsync(_options.GetQueueName("Delayed"), member).CA();

//        if (delay == null) return null;

//        var value = delay.Value;

//        return value > 0 ? value + TicksOffset : value;
//    }

//    public Task<JobDelay[]> GetDelaysByRangeAsync(Int64 minTicks, Int64 maxTicks, Boolean withTicks = false, Boolean ascending = true, Int64 skip = 0, Int64 take = -1)
//    {
//        throw new NotImplementedException();
//        //var key = _options.GetQueueName("Delayed");
//        //minTicks = minTicks > TicksOffset ? minTicks - TicksOffset : 0;
//        //maxTicks = maxTicks > TicksOffset ? maxTicks - TicksOffset : 0;
//        //var exclude = Exclude.None;
//        //var order = ascending ? Order.Ascending : Order.Descending;

//        //return withTicks ? (await GetDatabase().SortedSetRangeByScoreWithScoresAsync(key, minTicks, maxTicks, exclude, order, skip, take).CA()).To(JobDelayFromRedis)
//        //                 : (await GetDatabase().SortedSetRangeByScoreAsync(key, minTicks, maxTicks, exclude, order, skip, take).CA()).To(JobDelayFromRedis);
//    }

//    public Task<Boolean> DeleteDelayAsync(String name, String? arg, String? queue, Boolean repeat)
//    {
//        var member = DelayToRedisValue(name, arg, queue, repeat, out var db);
//        return db.SortedSetRemoveAsync(_options.GetQueueName("Delayed"), member);
//    }

//    public async Task<Boolean> DeleteAllDelaysAsync()
//    {
//        var deleted = false;
//        foreach (var db in _databases)
//        {
//            if (await db.KeyDeleteAsync(_options.GetQueueName("Delayed")).CA()) deleted = true;
//        }
//        return deleted;
//    }

//    #endregion IAsyncJobDelayer

//    #region IJobDelayer

//    public Boolean Delay(Int64 ticks, String name, String? arg, String? queue, Boolean repeat, Boolean? exists)
//    {
//        //Arg.GreaterThan(ticks, DateTime.UtcNow.Ticks);

//        ticks = ticks > TicksOffset ? ticks - TicksOffset : 0;

//        var member = DelayToRedisValue(name, arg, queue, repeat, out var db);

//        return db.SortedSetAdd(_options.GetQueueName("Delayed"), member, ticks, ToWhen(exists));
//    }

//    public Int64 GetCountDelays()
//    {
//        Int64 count = 0;
//        foreach (var db in _databases)
//        {
//            count += db.SortedSetLength(_options.GetQueueName("Delayed"), exclude: Exclude.Both);
//        }
//        return count;
//    }

//    public Int64? GetDelay(String name, String? arg, String? queue, Boolean repeat)
//    {
//        var member = DelayToRedisValue(name, arg, queue, repeat, out var db);

//        var delay = (Int64?)db.SortedSetScore(_options.GetQueueName("Delayed"), member);

//        if (delay == null) return null;

//        var value = delay.Value;

//        return value > 0 ? value + TicksOffset : value;
//    }

//    public JobDelay[] GetDelaysByRange(Int64 minTicks, Int64 maxTicks, Boolean withTicks = false, Boolean ascending = true, Int64 skip = 0, Int64 take = -1)
//    {
//        throw new NotImplementedException();
//        //var key = _options.GetQueueName("Delayed");
//        //minTicks = minTicks > TicksOffset ? minTicks - TicksOffset : 0;
//        //maxTicks = maxTicks > TicksOffset ? maxTicks - TicksOffset : 0;
//        //var exclude = Exclude.None;
//        //var order = ascending ? Order.Ascending : Order.Descending;

//        //return withTicks ? GetDatabase().SortedSetRangeByScoreWithScores(key, minTicks, maxTicks, exclude, order, skip, take).To(JobDelayFromRedis)
//        //                 : GetDatabase().SortedSetRangeByScore(key, minTicks, maxTicks, exclude, order, skip, take).To(JobDelayFromRedis);
//    }

//    public Boolean DeleteDelay(String name, String? arg, String? queue, Boolean repeat)
//    {
//        var member = DelayToRedisValue(name, arg, queue, repeat, out var db);
//        return db.SortedSetRemove(_options.GetQueueName("Delayed"), member);
//    }

//    public Boolean DeleteAllDelays()
//    {
//        var deleted = false;
//        foreach (var db in _databases)
//        {
//            if (db.KeyDelete(_options.GetQueueName("Delayed"))) deleted = true;
//        }
//        return deleted;
//    }

//    #endregion IJobDelayer

//    #region IAsyncJobAwaiter

//    public Task WaitAsync(Job parent, Job child)
//    {
//        throw new NotImplementedException();
//    }

//    public Task WaitAsync(params Job[] jobs)
//    {
//        Arg.NotNullEmpty(jobs);
//        Arg.MinLength(jobs, 2);

//        throw new NotImplementedException();
//    }

//    public Task WaitAllAsync(Job[] parents, Job job)
//    {
//        throw new NotImplementedException();
//    }

//    public Task WaitAnyAsync(Job[] parents, Job job)
//    {
//        throw new NotImplementedException();
//    }

//    #endregion IAsyncJobAwaiter

//    #region IJobAwaiter

//    public void Wait(Job parent, Job child)
//    {
//        throw new NotImplementedException();
//    }

//    public void Wait(params Job[] jobs)
//    {
//        Arg.NotNullEmpty(jobs);
//        Arg.MinLength(jobs, 2);

//        throw new NotImplementedException();
//    }

//    public void WaitAll(Job[] parents, Job job)
//    {
//        throw new NotImplementedException();
//    }

//    public void WaitAny(Job[] parents, Job job)
//    {
//        throw new NotImplementedException();
//    }

//    #endregion IJobAwaiter

//    #region IAsyncJobEnqueuer

//    public async Task<Boolean> EnqueueAsync(String name, String? arg, String? queue, Boolean repeat)
//    {
//        var value = ConvertToRedisValue(name, arg, ref queue, out var db);

//        if (repeat)
//        {
//            var status = GetJobStatus(await db.HashGetAsync($"{queue}:Status", value).CA());

//            if (status == null || status > JobStatus.Processing)
//            {
//                await db.SetStatusAsync(queue!, value, JobStatus.Enqueued).CA();
//                await db.ListLeftPushAsync($"{queue}:Enqueued", value).CA();
//                return true;
//            }
//        }
//        else
//        {
//            if (await db.SetStatusAsync(queue!, value, JobStatus.Enqueued, When.NotExists).CA())
//            {
//                await db.ListLeftPushAsync($"{queue}:Enqueued", value).CA();
//                return true;
//            }
//        }
//        return false;
//    }

//    public Task<Boolean[]> EnqueueAsync(params JobRepeat[] jobs) => throw new NotImplementedException();

//    #endregion IAsyncJobEnqueuer

//    #region IJobEnqueuer

//    public Boolean Enqueue(String name, String? arg, String? queue, Boolean repeat)
//    {
//        var value = ConvertToRedisValue(name, arg, ref queue, out var db);

//        if (repeat)
//        {
//            var status = GetJobStatus(db.HashGet($"{queue}:Status", value));

//            if (status == null || status > JobStatus.Processing)
//            {
//                db.SetStatus(queue!, value, JobStatus.Enqueued);
//                db.ListLeftPush($"{queue}:Enqueued", value);
//                return true;
//            }
//        }
//        else
//        {
//            if (db.SetStatus(queue!, value, JobStatus.Enqueued, When.NotExists))
//            {
//                db.ListLeftPush($"{queue}:Enqueued", value);
//                return true;
//            }
//        }
//        return false;
//    }

//    public Boolean[] Enqueue(params JobRepeat[] jobs) => throw new NotImplementedException();

//    #endregion IJobEnqueuer

//    #region IAsyncJobInformer

//    public Task<Boolean> ExistsAsync(String name, String? arg, String? queue)
//    {
//        var value = ConvertToRedisValue(name, arg, ref queue, out var db);
//        return db.HashExistsAsync($"{queue}:Status", value);
//    }

//    public async Task<JobStatus?> GetLastStatusAsync(String name, String? arg, String? queue)
//    {
//        var value = ConvertToRedisValue(name, arg, ref queue, out var db);
//        return GetJobStatus(await db.HashGetAsync($"{queue}:Status", value).CA());
//    }

//    public Task<Boolean> DeleteStatusAsync(String name, String? arg, String? queue)
//    {
//        var value = ConvertToRedisValue(name, arg, ref queue, out var db);
//        return db.HashDeleteAsync($"{queue}:Status", value);
//    }

//    public Task<Int64> DeleteStatusesAsync(params Job[] jobs)
//    {
//        Arg.NotNullEmpty(jobs);

//        //Int64 deleted = 0;

//        throw new NotImplementedException();

//        //foreach (var group in GroupByQueue(jobs))
//        //{
//        //    deleted += await _db.HashDeleteAsync($"{group.Key}:Status", group.Value.ToArray()).CA();
//        //}

//        //return deleted;
//    }

//    #endregion IAsyncJobInformer

//    #region IJobInformer

//    public Boolean Exists(String name, String? arg, String? queue)
//    {
//        var value = ConvertToRedisValue(name, arg, ref queue, out var db);
//        return db.HashExists($"{queue}:Status", value);
//    }

//    public JobStatus? GetLastStatus(String name, String? arg, String? queue)
//    {
//        var value = ConvertToRedisValue(name, arg, ref queue, out var db);
//        return GetJobStatus(db.HashGet($"{queue}:Status", value));
//    }

//    public Boolean DeleteStatus(String name, String? arg, String? queue)
//    {
//        var value = ConvertToRedisValue(name, arg, ref queue, out var db);
//        return db.HashDelete($"{queue}:Status", value);
//    }

//    public Int64 DeleteStatuses(params Job[] jobs)
//    {
//        Arg.NotNullEmpty(jobs);

//        //Int64 deleted = 0;

//        throw new NotImplementedException();

//        //foreach (var group in GroupByQueue(jobs))
//        //{
//        //    deleted += _db.HashDelete($"{group.Key}:Status", group.Value.ToArray());
//        //}

//        //return deleted;
//    }

//    #endregion IJobInformer

//    #region Private

//    //private IDatabase GetDatabase(String name, String? arg, String? queue)
//    //{
//    //    return _databaseFactory.GetRequired("ConnectionStrings:" + _options.ConnectionString);
//    //}

//    //private IDatabase GetDatabase() => _databases[0];

//    private IDatabase GetDatabaseByKey(String key)
//    {
//        throw new NotImplementedException();
//        //var bytes = _encoding.GetBytes(key);
//        //var hash = XXH64.DigestOf(bytes);
//        //var shard = hash % _shards;
//        //return _databases[shard];
//    }

//    private JobStatus? GetJobStatus(RedisValue value) => value.IsNull ? null : (JobStatus)(Int64)value;

//    private When ToWhen(Boolean? exists) => exists != null ? exists == true ? When.Exists : When.NotExists : When.Always;

//    //private Job JobFromRedis(ReadOnlySpan<Char> value)
//    //{
//    //    //{name}:{arg}:{queue}
//    //    var index = value.IndexOf(':');

//    //    if (index > -1)
//    //    {
//    //        var name = value.Slice(0, index).ToString();

//    //        //{arg}:{queue}
//    //        value = value.Slice(index + 1);

//    //        index = value.IndexOf(':');

//    //        if (index > -1)
//    //        {
//    //            var argSpan = value.Slice(0, index);

//    //            var arg = argSpan.IsEmpty ? null : argSpan.ToString();

//    //            //{queue}
//    //            var queueSpan = value.Slice(index + 1);

//    //            var queue = queueSpan.IsEmpty ? null : queueSpan.ToString();

//    //            return new Job(name) { Arg = arg, Queue = queue };
//    //        }
//    //        else return new Job(name) { Arg = value.ToString() };
//    //    }
//    //    else return new Job(value.ToString());
//    //}

//    //private JobDelay JobDelayFromRedisWith(RedisValue value, Int64 delay)
//    //{
//    //    var span = ((String)value).AsSpan();

//    //    var repeat = span[0] == '1';

//    //    var job = JobFromRedis(span.Slice(1));

//    //    return new JobDelay { Job = job, Repeat = repeat, Delay = delay };
//    //}

//    //private JobDelay JobDelayFromRedis(RedisValue value) => JobDelayFromRedisWith(value, 0);

//    //private JobDelay JobDelayFromRedis(SortedSetEntry entry)
//    //{
//    //    var delay = (Int64)entry.Score;

//    //    if (delay > 0) delay += TicksOffset;

//    //    return JobDelayFromRedisWith(entry.Element, delay);
//    //}

//    private RedisValue DelayToRedisValue(String name, String? arg, String? queue, Boolean repeat, out IDatabase db)
//    {
//        Arg.NotNull(name);
//        ValidArg(name);
//        if (arg != null) ValidArg(arg);
//        if (queue != null) ValidArg(queue);

//        var key = $"{name}:{arg}:{queue}";

//        db = GetDatabaseByKey(key);

//        return $"{(repeat ? 1 : 0)}{key}";
//    }

//    //private RedisValue JobToRedisValue(String name, String? arg, String? queue)
//    //{
//    //    Arg.NotNull(name);
//    //    ValidArg(name);
//    //    if (arg != null) ValidArg(arg);
//    //    if (queue != null) ValidArg(queue);

//    //    return $"{name}:{arg}:{queue}";
//    //}

//    //private RedisValue JobToRedisValue(Job job) => JobToRedisValue(job.Name, job.Arg, job.Queue);

//    //private HashEntry JobScheduleToHashEntry(JobSchedule jobSchedule, Int32 index)
//    //{
//    //    var job = jobSchedule.Job;

//    //    Arg.NotNull(job, paramName: $"schedules[{index}].Job");
//    //    ValidArg(job.Name, paramName: $"schedules[{index}].Job.Name");
//    //    if (job.Arg != null) ValidArg(job.Arg, paramName: $"schedules[{index}].Job.Arg");
//    //    if (job.Queue != null) ValidArg(job.Queue, paramName: $"schedules[{index}].Job.Queue");
//    //    Arg.NotNullEmptySpace(jobSchedule.Schedule, paramName: $"schedules[{index}].Schedule");

//    //    return new HashEntry($"{job.Name}:{job.Arg}:{job.Queue}", jobSchedule.Schedule);
//    //}

//    //private JobSchedule HashEntryToJobSchedule(HashEntry hashEntry) => new() { Job = JobFromRedis(((String)hashEntry.Name).AsSpan()), Schedule = hashEntry.Value };

//    //private HashEntry JobDelayToHashEntry(JobDelay jobDelay, Int32 index)
//    //{
//    //    var item = $"[{index}].";
//    //    Arg.NotNull(jobDelay.Job, paramName: item + nameof(jobDelay.Job));
//    //    Arg.GreaterThan(jobDelay.Delay, DateTime.UtcNow.Ticks, item + nameof(jobDelay.Delay));
//    //    return new HashEntry(JobToRedisValue(jobDelay.Job), jobDelay.Delay);
//    //}

//    private RedisValue ConvertToRedisValue(String name, String? arg, ref String? queue, out IDatabase db)
//    {
//        Arg.NotNull(name);
//        ValidArg(name);

//        if (queue is null)
//        {
//            if (_clientOptions.QueuePolicy == QueuePolicy.Individual)
//            {
//                if (arg is not null)
//                {
//                    ValidArg(arg!);
//                    db = GetDatabaseByKey($"{name}:{arg}:");
//                    queue = _options.GetQueueName(name);
//                    return arg;
//                }
//                db = GetDatabaseByKey($"{name}::");
//                queue = _options.GetQueueName(name);
//                return RedisValue.EmptyString;
//            }
//            else
//            {
//                queue = _clientOptions.GetSharedQueueName();
//            }
//        }
//        else
//        {
//            ValidArg(queue);
//        }

//        if (arg is not null)
//        {
//            ValidArg(arg!);
//            var nameArg = $"{name}:{arg}";
//            db = GetDatabaseByKey($"{nameArg}:{queue}");
//            queue = _options.GetQueueName(queue);
//            return nameArg;
//        }

//        db = GetDatabaseByKey($"{name}::{queue}");
//        queue = _options.GetQueueName(queue);
//        return name;
//    }

//    private static void ValidArg(String value, [CallerArgumentExpression("value")] String? paramName = null)
//    {
//        Arg.NotEmptySpace(value, paramName: paramName);
//        Arg.NotContains(value, ':', paramName: paramName);
//    }

//    #endregion Private
//}