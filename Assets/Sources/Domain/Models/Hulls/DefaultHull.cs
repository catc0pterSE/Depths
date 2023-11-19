using System;

namespace Sources.Domain.Models.Hulls
{
    public class DefaultHull : IHull
    {
        public DefaultHull(int impactDamage, int maxHealth)
        {
            ImpactDamage = impactDamage;
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        public event Action HealthChanged;
        public event Action Destroyed;

        public int ImpactDamage { get; }
        public int MaxHealth { get; }
        public int CurrentHealth { get; private set; }
        public bool IsDestroyed { get; private set; }

        public void TakeDamage(int damage)
        {
            if (damage < 0)
                throw new ArgumentException($"Invalid damage amount! Should be > 0, but invoked with: {damage}");

            CurrentHealth -= damage;
            HealthChanged?.Invoke();

            if (CurrentHealth <= 0)
            {
                IsDestroyed = true;
                Destroyed?.Invoke();
            }
        }
    }
}