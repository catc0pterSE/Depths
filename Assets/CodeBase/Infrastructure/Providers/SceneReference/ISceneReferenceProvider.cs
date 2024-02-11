using CodeBase.Infrastructure.SceneManagement;

namespace CodeBase.Infrastructure.Providers.SceneReference
{
    public interface ISceneReferenceProvider
    {
        string GetReference(ScenesEnum scenesEnum);
    }
}