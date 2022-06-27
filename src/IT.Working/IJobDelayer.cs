using System;

namespace IT.Working;

public interface IJobDelayer : IAsyncJobDelayer
{
    Boolean Delay(Int64 ticks, String name, String? arg = null, String? queue = null, Boolean repeat = false, Boolean? exists = null);

    Int64 GetCountDelays();

    Int64? GetDelay(String name, String? arg = null, String? queue = null, Boolean repeat = false);

    JobDelay[] GetDelaysByRange(Int64 minTicks, Int64 maxTicks, Boolean withTicks = false, Boolean ascending = true, Int64 skip = 0, Int64 take = -1);

    Boolean DeleteDelay(String name, String? arg = null, String? queue = null, Boolean repeat = false);

    Boolean DeleteAllDelays();
}