using System;
using System.Collections.Generic;
using Sources.Common.WindowFsm;
using Sources.Common.WindowFsm.Windows;
using Sources.Controllers.Api.Services;
using Sources.Controllers.Api.ViewModels;
using Sources.Controllers.Core.WindowFsms.Windows;
using Sources.Infrastructure.Api.GameFsm;

namespace Sources.Controllers.Core.ViewModels
{
    public class GameLoopViewModel : IDisposable, IGameLoopViewModel
    {
        private readonly IWindowFsm _windowFsm;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IGameLoopService _gameLoopService;

        private readonly Dictionary<Type, Action> _windowOpenReactions;
        private readonly Dictionary<Type, Action> _windowCloseReactions;

        public GameLoopViewModel
        (
            IWindowFsm windowFsm,
            IGameStateMachine gameStateMachine,
            IGameLoopService gameLoopService
        )
        {
            _windowFsm = windowFsm;
            _gameStateMachine = gameStateMachine;
            _gameLoopService = gameLoopService;

            _windowOpenReactions = new Dictionary<Type, Action>()
            {
                [typeof(RootWindow)] = () => InvokedRootShow?.Invoke(),
                [typeof(LoseWindow)] = () => InvokedLoseShow?.Invoke()
            };

            _windowCloseReactions = new Dictionary<Type, Action>()
            {
                [typeof(RootWindow)] = () => InvokedRootHide?.Invoke(),
                [typeof(LoseWindow)] = () => InvokedLoseHide?.Invoke(),
            };

            _windowFsm.Opened += OnWindowOpened;
            _windowFsm.Closed += OnWindowClosed;

            _gameLoopService.PlayerDied += OnPlayerDied;
            _gameLoopService.ScoreUpdated += OnScoreUpdated;
            _gameLoopService.BestScoreChanged += OnBestScoreUpdated;
        }

        public event Action InvokedRootShow;
        public event Action InvokedRootHide;
        public event Action InvokedLoseShow;
        public event Action InvokedLoseHide;
        public event Action<float> ScoreUpdated;
        public event Action<float> BestScoreUpdated;

        public void GoToMainMenu() => 
            _gameStateMachine.GoToMainMenu();

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            _windowFsm.Opened -= OnWindowOpened;
            _windowFsm.Closed -= OnWindowClosed;
            _gameLoopService.PlayerDied -= OnPlayerDied;
            _gameLoopService.ScoreUpdated -= OnScoreUpdated;
            _gameLoopService.BestScoreChanged -= OnScoreUpdated;
        }

        private void OnWindowOpened(IWindow window) => 
            _windowOpenReactions[window.GetType()].Invoke();
        
        private void OnWindowClosed(IWindow window) => 
            _windowCloseReactions[window.GetType()].Invoke();
        
        private void OnPlayerDied() => 
            _windowFsm.OpenWindow<LoseWindow>();
        
        private void OnScoreUpdated(float score) => 
            ScoreUpdated?.Invoke(score);
        
        private void OnBestScoreUpdated(float score) => 
            BestScoreUpdated?.Invoke(score);
    }
}