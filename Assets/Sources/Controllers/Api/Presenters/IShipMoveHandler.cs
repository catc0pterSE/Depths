using Sources.Domain.Models.Ships;

namespace Sources.Controllers.Api.Presenters
{
    public interface IShipMoveHandler
    {
        void Initialize(IShip ship);
        void Enable();
        void Disable();
    }
}