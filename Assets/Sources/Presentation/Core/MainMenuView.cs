using Sources.Controllers.Api.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presentation.Core
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private ScoreView _bestScoreView;

        private IMainMenuViewModel _mainMenuViewModel;
        private bool _isInitialized;

        public void Initialize(IMainMenuViewModel mainMenuViewModel)
        {
            _mainMenuViewModel = mainMenuViewModel;

            _startButton.onClick.AddListener(_mainMenuViewModel.StartGameLoop);

            _bestScoreView.Initialize();
            _bestScoreView.Render(mainMenuViewModel.GetBestScore());

            _isInitialized = true;
        }

        private void OnDestroy()
        {
            if (_isInitialized == false)
                return;

            _startButton.onClick.RemoveListener(_mainMenuViewModel.StartGameLoop);
        }
    }
}