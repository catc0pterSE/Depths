using System;

namespace Sources.Controllers.Api.ViewModels
{
    public interface IGameLoopViewModel
    {
        event Action InvokedRootShow;
        event Action InvokedRootHide;
        event Action InvokedLoseShow;
        event Action InvokedLoseHide;
        event Action<float> ScoreUpdated;
        event Action<float> BestScoreUpdated;
        
        void GoToMainMenu();
    }
}