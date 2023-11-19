using System;

namespace Sources.Controllers.Api.Services
{
    public interface IGameLoopService
    {
        event Action PlayerDied;
        event Action<float> ScoreUpdated;
        event Action<float> BestScoreChanged;
        
        void Start();
        void Stop();
        void Update();
    }
}