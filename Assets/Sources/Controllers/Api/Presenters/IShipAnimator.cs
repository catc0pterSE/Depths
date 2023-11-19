using System;
using Sources.Domain.Models.Ships;

namespace Sources.Controllers.Api.Presenters
{
    public interface IShipAnimator
    {
        event Action ExplosionCompleted;
        
        void Initialize(IShip ship);
        void PlayExhaust();
        void StopExhaust();
        void PlayExplosion();
        void Reset();
    }
}