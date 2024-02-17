using System;
using System.Collections;
using System.Threading.Tasks;
using CodeBase.Utility.Extensions;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Loading
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _curtain;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _fadingStep = 0.01f;

        private float _defaultCurtainAlpha;
        private Coroutine _fadeJob;

        private void Awake()
        {
            _defaultCurtainAlpha = _curtain.alpha;
            DontDestroyOnLoad(this);
        }

        public void ShowInstant(string text = default)
        {
            SetText(text);
            
            _curtain.alpha = _defaultCurtainAlpha;
            this.EnableObject();
        }

        public async UniTask FadeInAsync()
        {
            this.EnableObject();
            await Fade(_defaultCurtainAlpha);
        }

        public async UniTask FadeInFadeOut(Action toDoWhenFaded = null, string text = default)
        {
            SetText(text);

            await FadeInAsync();

            if (toDoWhenFaded is not null)
                await new Task(toDoWhenFaded.Invoke);

            await FadeOutAsync();
        }

        public async UniTask FadeOutAsync()
        {
            await Fade(0);
            this.DisableObject();
        }

        private void SetText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                _text.text = text;
                _text.EnableObject();
            }
            else
            {
                _text.DisableObject();
            }
        }

        private IEnumerator Fade(float targetAlpha)
        {
            while (Math.Abs(_curtain.alpha - targetAlpha) > Mathf.Epsilon)
            {
                _curtain.alpha = Mathf.MoveTowards(_curtain.alpha, targetAlpha, _fadingStep);
                yield return null;
            }
        }
    }
}