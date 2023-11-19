using Sources.Controllers.Api.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presentation.Core
{
    public class LoseView : MonoBehaviour
    {
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private Button _menuButton;

        private IGameLoopViewModel _gameLoopViewModel;

        private bool _isInitialized;

        public void Initialize(IGameLoopViewModel gameLoopViewModel)
        {
            _gameLoopViewModel = gameLoopViewModel;

            _gameLoopViewModel.InvokedLoseShow += Show;
            _gameLoopViewModel.InvokedLoseHide += Hide;

            _menuButton.onClick.AddListener(_gameLoopViewModel.GoToMainMenu);

            _isInitialized = true;
        }

        private void OnDestroy()
        {
            if (_isInitialized == false)
                return;

            _gameLoopViewModel.InvokedLoseShow -= Show;
            _gameLoopViewModel.InvokedLoseHide -= Hide;

            _menuButton.onClick.RemoveListener(_gameLoopViewModel.GoToMainMenu);
        }

        private void Show() =>
            _mainCanvas.enabled = true;

        private void Hide() =>
            _mainCanvas.enabled = false;
    }
}