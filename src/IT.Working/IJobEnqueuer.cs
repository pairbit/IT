using System;

namespace IT.Working;

public interface IJobEnqueuer : IAsyncJobEnqueuer
{
    Boolean Enqueue(String name, String? arg = null, String? queue = null, Boolean repeat = false);

    Boolean[] Enqueue(params JobRepeat[] jobs);
}