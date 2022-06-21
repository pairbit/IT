using System;
using System.Threading.Tasks;

namespace IT.Locking;

public interface ILock : IDisposable, IAsyncDisposable
{
    void Unlock();

    ValueTask UnlockAsync();
}