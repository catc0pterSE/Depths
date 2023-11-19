using System;
using Sources.Controllers.Api.Presenters;
using Sources.Domain.Models.Ships;
using UnityEngine;

namespace Sources.Controllers.Core.Presenters
{
    [RequireComponent(typeof(IShipCollisionHandler))]
    [RequireComponent(typeof(IShipMoveHandler))]
    [RequireComponent(typeof(IShipAnimator))]
    public class ShipPresenter : MonoBehaviour, IShipPresenter
    {
        private IShipCollisionHandler _shipCollisionHandler;
        private IShipMoveHandler _shipMoveHandler;
        private IShipAnimator _shipAnimator;
        private IShip _ship;

        private bool _isInitialized;

        public event Action<IShipPresenter> Destroyed;
        public event Action<IShipPresenter> ReadyToRelease;

        public Transform Transform => transform;
        public bool InUse { get; private set; }

        public void Initialize(IShip ship)
        {
            gameObject.SetActive(true);

            _ship = ship;
            _shipCollisionHandler = GetComponent<IShipCollisionHandler>();
            _shipMoveHandler = GetComponent<IShipMoveHandler>();
            _shipAnimator = GetComponent<IShipAnimator>();

            _shipCollisionHandler.Initialize(_ship);
            _shipMoveHandler.Initialize(_ship);
            _shipAnimator.Initialize(_ship);

            Subscribe();

            _shipCollisionHandler.Enable();
            _shipMoveHandler.Enable();

            _shipAnimator.Reset();
            _shipAnimator.PlayExhaust();

            InUse = true;
            _isInitialized = true;
        }

        private void OnDestroy()
        {
            if (_isInitialized == false)
                return;

            Unsubscribe();
        }

        public void HandleImpact(IShip impactInitiator) =>
            _ship.Impact(impactInitiator);

        public void Release()
        {
            InUse = false;

            _shipCollisionHandler.Disable();
            _shipMoveHandler.Disable();

            Unsubscribe();
            ReadyToRelease?.Invoke(this);
            gameObject.SetActive(false);
        }

        private void Subscribe()
        {
            _shipCollisionHandler.Impacted += OnImpacted;
            _shipAnimator.ExplosionCompleted += OnExplosionCompleted;
        }

        private void Unsubscribe()
        {
            _shipCollisionHandler.Impacted -= OnImpacted;
            _shipAnimator.ExplosionCompleted -= OnExplosionCompleted;
        }

        private void OnImpacted(IShipPresenter impactTarget) =>
            impactTarget.HandleImpact(_ship);

        private void OnExplosionCompleted()
        {
            Destroyed?.Invoke(this);
            Release();
        }
    }
}