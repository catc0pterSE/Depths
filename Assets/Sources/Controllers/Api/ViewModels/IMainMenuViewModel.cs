using System;

namespace Sources.Controllers.Api.ViewModels
{
    public interface IMainMenuViewModel
    {
        event Action InvokedRootShow;
        event Action InvokedRootHide;
        
        void StartGameLoop();
        float GetBestScore();
    }
}