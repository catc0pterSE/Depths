namespace Sources.Controllers.Api.Presenters.Strategies
{
    public interface IStrategy
    {
        void Enable();
        void Disable();
        void Resolve();
    }
}