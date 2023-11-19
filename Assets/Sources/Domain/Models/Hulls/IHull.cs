using System;

namespace Sources.Domain.Models.Hulls
{
    public interface IHull
    {
        event Action HealthChanged;
        event Action Destroyed; 
        
        int MaxHealth { get; }
        int CurrentHealth { get; }
        int ImpactDamage { get; }
        bool IsDestroyed { get; }
        
        void TakeDamage(int damage);
    }
}