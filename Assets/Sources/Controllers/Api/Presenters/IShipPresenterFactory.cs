namespace Sources.Controllers.Api.Presenters
{
    public interface IShipPresenterFactory
    {
        IShipPresenter CreatePlayerShipPresenter();
        IShipPresenter CreateEnemyShipPresenter();
    }
}