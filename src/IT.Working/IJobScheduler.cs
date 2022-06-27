using System;

namespace IT.Working;

public interface IJobScheduler : IAsyncJobScheduler
{
    Boolean Schedule(String schedule, String name, String? arg = null, String? queue = null, Boolean update = true);

    void Schedule(params JobSchedule[] schedules);

    Boolean ExistsSchedule(String name, String? arg = null, String? queue = null);

    Int64 GetCountSchedules();

    String GetSchedule(String name, String? arg = null, String? queue = null);

    String[] GetSchedules(params Job[] jobs);

    JobSchedule[] GetAllSchedules();

    Boolean DeleteSchedule(String name, String? arg = null, String? queue = null);

    Int64 DeleteSchedules(params Job[] jobs);

    Boolean DeleteAllSchedules();
}