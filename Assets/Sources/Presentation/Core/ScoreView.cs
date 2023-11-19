using System;
using TMPro;
using UnityEngine;

namespace Sources.Presentation.Core
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;

        private string _prefix;
        private string _postfix;
        private string _scoreFormat;

        public void Initialize(string prefix = null, string postfix = null, string scoreFormat = "f2")
        {
            _prefix = prefix ?? String.Empty;
            _postfix = postfix ?? String.Empty;
            _scoreFormat = scoreFormat;
        }

        public void Render(float score) => 
            _scoreText.text = $"{_prefix}{score.ToString(_scoreFormat)}{_postfix}";
    }
}