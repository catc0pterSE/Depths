using System;

namespace Sources.Infrastructure.Api
{
    public interface IPoolable<out T>
    {
        event Action<T> ReadyToRelease;

        void Release();
    }
}