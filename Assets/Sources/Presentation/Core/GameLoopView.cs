using Sources.Controllers.Api.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Presentation.Core
{
    public class GameLoopView : MonoBehaviour
    {
        [SerializeField] private Button _menuButton;
        [SerializeField] private ScoreView _currentScoreView;
        [SerializeField] private ScoreView _bestScoreView;

        private const string CurrentScorePrefix = "Progress: ";
        private const string BestScorePrefix = "Best: ";
        private const string ScorePostfix = "s";

        private IGameLoopViewModel _gameLoopViewModel;

        private bool _isInitialized;

        public void Initialize(IGameLoopViewModel gameLoopViewModel)
        {
            _gameLoopViewModel = gameLoopViewModel;

            _currentScoreView.Initialize(CurrentScorePrefix, ScorePostfix);
            _bestScoreView.Initialize(BestScorePrefix, ScorePostfix);

            _gameLoopViewModel.ScoreUpdated += UpdateScore;
            _gameLoopViewModel.BestScoreUpdated += UpdateBestScore;

            _menuButton.onClick.AddListener(_gameLoopViewModel.GoToMainMenu);

            _isInitialized = true;
        }

        private void OnDestroy()
        {
            if (_isInitialized == false)
                return;

            _gameLoopViewModel.ScoreUpdated -= UpdateScore;
            _gameLoopViewModel.BestScoreUpdated -= UpdateBestScore;

            _menuButton.onClick.RemoveListener(_gameLoopViewModel.GoToMainMenu);
        }

        private void UpdateScore(float score) => 
            _currentScoreView.Render(score);

        private void UpdateBestScore(float score) => 
            _bestScoreView.Render(score);
    }
}