using System;
using System.Threading.Tasks;

namespace IT.Working;

public static class xIAsyncJobEnqueuer
{
    public static Task<Boolean> EnqueueAsync(this IAsyncJobEnqueuer jobEnqueuer, Job job, Boolean repeat = false)
    {
        if (job is null) throw new ArgumentException(nameof(job));
        return jobEnqueuer.EnqueueAsync(job.Name, job.Arg, job.Queue, repeat);
    }

    /// <summary>
    /// EnqueueAsync($"{typeof(TType).FullName}.{methodName}", arg)
    /// </summary>
    //public static Task EnqueueAsync<TType>(this IAsyncJobService client, String methodName, String arg = null, String queue = null)
    //    => client.EnqueueAsync($"{typeof(TType).FullName}.{methodName}", arg, queue);

    //private readonly static ConcurrentDictionary<String, Delegate[]> Delegates = new();

    //public static Task EnqueueAsync<TType>(this IAsyncJobService client, Expression<Action<TType>> expression, String queue = null, Boolean cache = true)
    //{
    //    throw new NotSupportedException("");
    //}

    //public static Task EnqueueAsync<TType>(this IAsyncWorkerClient client, Expression<Action<TType>> expression, String queue = null, Boolean cache = true)
    //{
    //    Arg.NotNull(expression);
    //    Arg.NotNull(expression.Body);
    //    Arg.Valid(expression.NodeType == ExpressionType.Lambda);

    //    if (expression.Body is MethodCallExpression methodCall)
    //    {
    //        Arg.Valid(methodCall.NodeType == ExpressionType.Call);
    //        Arg.NotNull(methodCall.Method);

    //        var fullName = methodCall.Method.GetFullName();

    //        var arguments = methodCall.Arguments;

    //        if (arguments.IsEmpty()) return client.EnqueueAsync(fullName, queue);

    //        var args = cache ? Delegates.GetOrAdd(fullName, GetDelegateArguments, arguments).To(x => x.DynamicInvoke())
    //                         : arguments.To(x => Expression.Lambda(x).Compile().DynamicInvoke());

    //        return args.Length == 1 ? client.EnqueueAsync(fullName, args[0], queue) : client.EnqueueAsync(fullName, queue, args);
    //    }
    //    throw new NotSupportedException($"Expression '{expression}' with NodeType is '{expression.NodeType}' and Type is '{expression.GetType().FullName}'");
    //}

    //private static Delegate[] GetDelegateArguments(String fullName, IReadOnlyList<Expression> arguments)
    //{
    //    var args = new Delegate[arguments.Count];

    //    for (int i = 0; i < args.Length; i++)
    //    {
    //        var argument = arguments[i];
    //        var lambda = Expression.Lambda(argument);
    //        args[i] = lambda.Compile();
    //    }

    //    return args;
    //}
}