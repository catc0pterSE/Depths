using System;

namespace Sources.Controllers.Api.Services
{
    public interface ILevelProgressCounter
    {
        event Action<float> Updated;
        
        float Value { get; }
        
        void Start();
        void Update(float deltaTime);
        void Stop();
    }
}