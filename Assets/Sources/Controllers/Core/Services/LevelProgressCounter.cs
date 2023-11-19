using System;
using Sources.Controllers.Api.Services;

namespace Sources.Controllers.Core.Services
{
    public class LevelProgressCounter : ILevelProgressCounter
    {
        private float _currentTime;

        public event Action<float> Updated;

        public float Value => _currentTime;

        public void Start()
        {
            _currentTime = 0;
        }

        public void Update(float deltaTime)
        {
            _currentTime += deltaTime;
            Updated?.Invoke(_currentTime);
        }

        public void Stop()
        {
            Updated?.Invoke(_currentTime);
        }
    }
}