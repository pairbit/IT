using System;

namespace IT.Working;

public interface IJobInformer
{
    Boolean Exists(String name, String? arg = null, String? queue = null);

    JobStatus? GetLastStatus(String name, String? arg = null, String? queue = null);

    Boolean DeleteStatus(String name, String? arg = null, String? queue = null);

    Int64 DeleteStatuses(params Job[] jobs);

    //WorkState[]? GetStates(params Job[] jobs);

    //Boolean Exists(String name, String? arg = null, String? queue = null);

    //Boolean[] Exists(params Job[] jobs);
}