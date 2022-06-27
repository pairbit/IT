using System;
using System.Threading.Tasks;

namespace IT.Working;

public interface IAsyncJobDelayer
{
    Task<Boolean> DelayAsync(Int64 ticks, String name, String? arg = null, String? queue = null, Boolean repeat = false, Boolean? exists = null);

    Task<Int64> GetCountDelaysAsync();

    Task<Int64?> GetDelayAsync(String name, String? arg = null, String? queue = null, Boolean repeat = false);

    Task<JobDelay[]> GetDelaysByRangeAsync(Int64 minTicks, Int64 maxTicks, Boolean withTicks = false, Boolean ascending = true, Int64 skip = 0, Int64 take = -1);

    Task<Boolean> DeleteDelayAsync(String name, String? arg = null, String? queue = null, Boolean repeat = false);

    Task<Boolean> DeleteAllDelaysAsync();
}