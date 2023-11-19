using System;
using System.Collections.Generic;
using Sources.Common.WindowFsm;
using Sources.Common.WindowFsm.Windows;
using Sources.Controllers.Api.Services;
using Sources.Controllers.Core.ViewModels;
using Sources.Controllers.Core.WindowFsms;
using Sources.Controllers.Core.WindowFsms.Windows;
using Sources.Infrastructure.Api.GameFsm;
using Sources.Infrastructure.Core.Services.DI;
using Sources.Presentation.Core;
using UnityEngine;

namespace Sources.Application.CompositionRoots
{
    public sealed class MainMenuCompositionRoot : SceneCompositionRoot
    {
        [SerializeField] private MainMenuView _mainMenuView;

        public override void Initialize(ServiceContainer serviceContainer)
        {
            Dictionary<Type, IWindow> windows = new Dictionary<Type, IWindow>()
            {
                [typeof(RootWindow)] = new RootWindow(),
            };
            
            IWindowFsm windowFsm = new WindowFsm<RootWindow>(windows);
            IGameStateMachine gameStateMachine = serviceContainer.Single<IGameStateMachine>();
            IPersistentDataService persistentDataService = serviceContainer.Single<IPersistentDataService>();
          
            MainMenuViewModel mainMenuViewModel = new MainMenuViewModel(windowFsm, gameStateMachine, persistentDataService);

            _mainMenuView.Initialize(mainMenuViewModel);
        }
    }
}