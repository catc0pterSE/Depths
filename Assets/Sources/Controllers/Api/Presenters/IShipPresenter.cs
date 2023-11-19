using System;
using Sources.Domain.Models.Ships;
using Sources.Infrastructure.Api;
using UnityEngine;

namespace Sources.Controllers.Api.Presenters
{
    public interface IShipPresenter : IPoolable<IShipPresenter>
    {
        event Action<IShipPresenter> Destroyed;
        
        Transform Transform { get; }
        bool InUse { get; }
        
        void Initialize(IShip ship);
        void HandleImpact(IShip impactInitiator);
    }
}