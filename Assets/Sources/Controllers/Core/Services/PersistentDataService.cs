using System;
using System.Globalization;
using Sources.Controllers.Api.Services;
using Sources.Infrastructure.Api.Services;

namespace Sources.Controllers.Core.Services
{
    public class PersistentDataService : IPersistentDataService
    {
        private const string BestScoreKey = "BestScore";

        private readonly ISaveService _saveService;

        public PersistentDataService(ISaveService saveService)
        {
            _saveService = saveService;
        }

        public event Action<float> BestScoreChanged;

        public void SetBestScore(float score)
        {
            float currentScore = GetBestScore();
            if (currentScore >= score)
                return;

            _saveService.Save(BestScoreKey, score.ToString(CultureInfo.InvariantCulture));
            BestScoreChanged?.Invoke(score);
        }

        public float GetBestScore()
        {
            string bestScoreString = _saveService.Get(BestScoreKey);
            return float.TryParse(bestScoreString, NumberStyles.Any, CultureInfo.InvariantCulture, out float bestScore)
                ? bestScore
                : 0;
        }

        public void ClearData() => 
            _saveService.Clear();
    }
}