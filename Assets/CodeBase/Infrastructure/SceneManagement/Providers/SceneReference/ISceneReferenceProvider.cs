namespace CodeBase.Infrastructure.SceneManagement.Providers.SceneReference
{
    public interface ISceneReferenceProvider
    {
        string GetReference(ScenesEnum scenesEnum);
    }
}