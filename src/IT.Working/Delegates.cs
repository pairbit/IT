using System;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Working;

//public delegate void Work(String arg, CancellationToken cancellationToken);
//public delegate WorkResult WorkWithResult(String arg, CancellationToken cancellationToken);

public delegate Task WorkNoResultAsync(String arg, CancellationToken cancellationToken);
public delegate Task<WorkResult> WorkAsync(String arg, CancellationToken cancellationToken);