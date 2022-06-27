using System;
using System.Threading.Tasks;

namespace IT.Working;

public interface IAsyncJobScheduler
{
    Task<Boolean> ScheduleAsync(String schedule, String name, String? arg = null, String? queue = null, Boolean update = true);

    Task ScheduleAsync(params JobSchedule[] schedules);

    Task<Boolean> ExistsScheduleAsync(String name, String? arg = null, String? queue = null);

    Task<Int64> GetCountSchedulesAsync();

    Task<String> GetScheduleAsync(String name, String? arg = null, String? queue = null);

    Task<String[]> GetSchedulesAsync(params Job[] jobs);

    Task<JobSchedule[]> GetAllSchedulesAsync();

    Task<Boolean> DeleteScheduleAsync(String name, String? arg = null, String? queue = null);

    Task<Int64> DeleteSchedulesAsync(params Job[] jobs);

    Task<Boolean> DeleteAllSchedulesAsync();
}