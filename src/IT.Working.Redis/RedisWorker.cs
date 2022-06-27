//using IT.Design;
//using IT.Linq;
//using IT.Redis.Working.Options;
//using IT.Reflection;
//using IT.Working;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using StackExchange.Redis;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Reflection;
//using System.Threading;
//using System.Threading.Tasks;

//namespace IT.Working.Redis;

//public class RedisWorker : IAsyncWorker
//{
//    private readonly IFactory<WorkAsync> _workFactory;
    
//    private readonly ILogger _logger;
//    private readonly IServiceProvider _serviceProvider;
//    private readonly UInt64 _shards;
//    private readonly IDatabase[] _databases;
//    private readonly RedisWorkingOptions _baseOptions;
//    private readonly RedisWorkerOptions _options;

//    public RedisWorker(
//        IServiceProvider serviceProvider,
//        IOptionsSnapshot<RedisWorkingOptions> baseOptions,
//        IOptionsSnapshot<RedisWorkerOptions> options,
//        IFactory<WorkAsync> workFactory,
//        IFactory<IDatabase> databaseFactory,
//        ILogger<RedisWorker> logger)
//    {
//        _workFactory = workFactory;
//        _serviceProvider = serviceProvider;
//        _baseOptions = baseOptions.Value;
//        _options = options.Value;
//        _databases = _baseOptions.ConnectionString.Split(';').To(x => databaseFactory.GetRequired("ConnectionStrings:" + x));
//        _shards = (UInt64)_databases.Length;
//        _logger = logger;
//    }

//    public async Task WorkAsync(CancellationToken cancellationToken)
//    {
//        try
//        {
//            _logger.LogInformation($"Worker running at: {DateTimeOffset.Now}");

//            var tasks = new List<Task>();

//            var machineName = Environment.MachineName;
//            //WorkAsync workAsync

//            var queues = _options.Queues[machineName];

//            foreach (var queue in queues)
//            {
//                var queueName = queue.Key;
//                var queueOptions = queue.Value;
//                var workAsync = _workFactory.GetRequired(queueName);

//                //Для дебага, если запустить больше одного потока, будет мешаться отладке
//                var workers = Debugger.IsAttached ? 1 : queueOptions.Workers;

//                for (int i = 0; i < _databases.Length; i++)
//                {
//                    var db = _databases[i];
//                    var dbNum = i;

//                    for (int num = workers - 1; num >= 0; num--)
//                    {
//                        var numpad = num.ToString().PadLeft(3, '0');
//                        var name = $"{machineName}_{queueName}_{dbNum}_{num}";
//                        var id = Id.NewBase64();
//                        tasks.Add(RunTaskAsync(workAsync, id, name, db, queueName, cancellationToken));
//                    }
//                }
//            }

//            await Task.WhenAll(tasks).CA();

//            _logger.LogInformation("All workers stopped");
//        }
//        catch (Exception ex)
//        {
//            _logger.LogCritical(ex, "Error Redis Worker");
//            throw;
//        }
//    }

//    private async Task RunTaskAsync(WorkAsync workAsync, String id, String name, IDatabase db, String queueName, CancellationToken cancellationToken)
//    {
//        var idname = $"{name}_{id}";
//        try
//        {
//            _logger.LogInformation($" [+] Start worker ({idname}) ");
//            while (!cancellationToken.IsCancellationRequested)
//            {
//                var value = await db.DequeueAsync(queueName, id).CA();

//                if (value.IsNull)
//                {
//                    await Task.Delay(1000, cancellationToken).CA();
//                    continue;
//                }

//                String arg = value;

//                using (_logger.BeginScope($"Process {arg} from {queueName}", arg, queueName, id))
//                {
//                    try
//                    {
//                        var result = await workAsync(arg, cancellationToken).CA();

//                        if (result == WorkResult.Rollback) await db.QueueRollbackAsync(queueName, id).CA();

//                        else if (result == WorkResult.Deleted)
//                        {
//                            await db.QueueAddToDeletedAsync(queueName, id, value).CA();
//                        }
//                        else if (result == WorkResult.Processed)
//                        {
//                            await db.QueueAddToSucceededAsync(queueName, id, value).CA();
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        ex.Data.Add(nameof(queueName), queueName);
//                        ex.Data.Add(nameof(arg), arg);
//                        ex.Data.Add(nameof(id), id);

//                        _logger.LogError(ex, "Ошибка обработки");

//                        await db.QueueAddToFailedAsync(queueName, id, value).CA();
//                    }
//                }
//            }
//            _logger.LogInformation($" [-] Finished worker ({idname})");
//        }
//        catch (TaskCanceledException)
//        {
//            _logger.LogInformation($" [-] Canceled worker ({idname})");
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, $"[-] Error worker ({idname})");
//        }
//    }
//}