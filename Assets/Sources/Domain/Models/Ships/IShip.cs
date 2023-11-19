using System;

namespace Sources.Domain.Models.Ships
{
    public interface IShip
    {
        public event Action HealthChanged;
        public event Action Destroyed;
        
        int Speed { get; }
        int MaxHealth { get; }
        int CurrentHealth { get; }
        bool IsDestroyed { get; }
        
        void Attack(IShip ship);
        void TakeDamage(int damage);
        void Impact(IShip ship);
    }
}