namespace Sources.Domain.Models.Weapons
{
    public class Weapon : IWeapon
    {
        public Weapon(int damage)
        {
            Damage = damage;
        }

        public int Damage { get; }
    }
}