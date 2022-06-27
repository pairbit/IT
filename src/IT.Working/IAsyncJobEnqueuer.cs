using System;
using System.Threading.Tasks;

namespace IT.Working;

public interface IAsyncJobEnqueuer
{
    Task<Boolean> EnqueueAsync(String name, String? arg = null, String? queue = null, Boolean repeat = false);

    Task<Boolean[]> EnqueueAsync(params JobRepeat[] jobs);
}