using System;
using System.Threading.Tasks;

namespace IT.Working;

public interface IAsyncJobInformer
{
    Task<Boolean> ExistsAsync(String name, String? arg = null, String? queue = null);

    Task<JobStatus?> GetLastStatusAsync(String name, String? arg = null, String? queue = null);

    Task<Boolean> DeleteStatusAsync(String name, String? arg = null, String? queue = null);

    Task<Int64> DeleteStatusesAsync(params Job[] jobs);
}