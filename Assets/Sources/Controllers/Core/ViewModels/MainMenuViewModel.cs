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
    public class MainMenuViewModel : IDisposable, IMainMenuViewModel
    {
        private readonly IWindowFsm _windowFsm;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IPersistentDataService _persistentDataService;

        private readonly Dictionary<Type, Action> _windowOpenReactions;
        private readonly Dictionary<Type, Action> _windowCloseReactions;

        public MainMenuViewModel
        (
            IWindowFsm windowFsm,
            IGameStateMachine gameStateMachine,
            IPersistentDataService persistentDataService
        )
        {
            _windowFsm = windowFsm;
            _gameStateMachine = gameStateMachine;
            _persistentDataService = persistentDataService;

            _windowOpenReactions = new Dictionary<Type, Action>()
            {
                [typeof(RootWindow)] = () => InvokedRootShow?.Invoke()
            };

            _windowCloseReactions = new Dictionary<Type, Action>()
            {
                [typeof(RootWindow)] = () => InvokedRootHide?.Invoke()
            };

            _windowFsm.Opened += OnWindowOpened;
            _windowFsm.Closed += OnWindowClosed;
        }

        public event Action InvokedRootShow;
        public event Action InvokedRootHide;

        public void StartGameLoop() => 
            _gameStateMachine.GoToGameLoop();

        public float GetBestScore() => 
            _persistentDataService.GetBestScore();

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            _windowFsm.Opened -= OnWindowOpened;
            _windowFsm.Closed -= OnWindowClosed;
        }

        private void OnWindowOpened(IWindow window) => 
            _windowOpenReactions[window.GetType()].Invoke();

        private void OnWindowClosed(IWindow window) => 
            _windowCloseReactions[window.GetType()].Invoke();
    }
}