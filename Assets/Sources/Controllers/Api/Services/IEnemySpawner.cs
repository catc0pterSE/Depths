using Sources.Controllers.Api.Presenters;
using UnityEngine;

namespace Sources.Controllers.Api.Services
{
    public interface IEnemySpawner
    {
        IShipPresenter SpawnAt(Vector3 position, Quaternion rotation);
        void Update();
        void Enable();
        void Disable();
    }
}