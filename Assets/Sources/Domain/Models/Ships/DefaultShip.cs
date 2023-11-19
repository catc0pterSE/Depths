using System;
using Sources.Domain.Models.Enginies;
using Sources.Domain.Models.Hulls;
using Sources.Domain.Models.Weapons;

namespace Sources.Domain.Models.Ships
{
    public class Ship : IShip
    {
        private readonly IHull _hull;
        private readonly IWeapon _weapon;
        private readonly IEngine _engine;

        public Ship(IHull hull, IWeapon weapon, IEngine engine)
        {
            _hull = hull;
            _weapon = weapon;
            _engine = engine;

            _hull.HealthChanged += OnHealthChanged;
            _hull.Destroyed += OnDestroyed;
        }

        ~Ship()
        {
            _hull.HealthChanged -= OnHealthChanged;
            _hull.Destroyed -= OnDestroyed;
        }

        public event Action HealthChanged;
        public event Action Destroyed;

        public int Speed => _engine.Speed;
        public int MaxHealth => _hull.MaxHealth;
        public int CurrentHealth => _hull.CurrentHealth;
        public bool IsDestroyed => _hull.IsDestroyed;

        public void Attack(IShip ship) => 
            ship.TakeDamage(_weapon.Damage);
        
        public void Impact(IShip ship) => 
            ship.TakeDamage(_hull.ImpactDamage);
        
        public void TakeDamage(int damage) => 
            _hull.TakeDamage(damage);
        
        private void OnHealthChanged() => 
            HealthChanged?.Invoke();
        
        private void OnDestroyed() => 
            Destroyed?.Invoke();
    }
}