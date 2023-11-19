using System;

namespace Sources.Controllers.Api.Services
{
    public interface IPersistentDataService
    {
        event Action<float> BestScoreChanged;
        
        float GetBestScore();
        void SetBestScore(float score);
        void ClearData();
    }
}