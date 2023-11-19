using System;
using Sources.Domain.Models.Ships;

namespace Sources.Controllers.Api.Presenters
{
    public interface IShipCollisionHandler
    {
        event Action<IShipPresenter> Impacted;
        
        void Initialize(IShip ship);
        void Enable();
        void Disable();
    }
}