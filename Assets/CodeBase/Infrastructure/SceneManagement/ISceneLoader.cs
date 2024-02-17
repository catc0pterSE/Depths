using System;

namespace CodeBase.Infrastructure.SceneManagement
{
    public interface ISceneLoader
    {
        void LoadScene(ScenesEnum scene, Action onLoaded = null);
    }
}