using Sources.Domain.Builders;
using Sources.Domain.Models.Enginies;
using Sources.Domain.Models.Hulls;
using Sources.Domain.Models.Ships;
using Sources.Domain.Models.Weapons;

namespace Sources.Domain.Factories
{
    public class ShipFactory : IShipFactory
    {
        private static int DefaultShipImpactDamage = 3;
        private static int DefaultShipHealthPoints = 3;
        private static int DefaultShipSpeed = 3;
        private static int DefaultShipDamage = 3;

        private static int WeakShipImpactDamage = 1;
        private static int WeakShipHealthPoints = 3;
        private static int WeakShipSpeed = 3;
        private static int WeakShipDamage = 3;

        public IShip CreateDefault() => 
            new ShipBuilder()
                .SetHull(new DefaultHull(DefaultShipImpactDamage, DefaultShipHealthPoints))
                .SetEngine(new Engine(DefaultShipSpeed))
                .SetWeapon(new Weapon(DefaultShipDamage))
                .Build();

        public IShip CreateWeak() => 
            new ShipBuilder()
                .SetHull(new DefaultHull(WeakShipImpactDamage, WeakShipHealthPoints))
                .SetEngine(new Engine(WeakShipSpeed))
                .SetWeapon(new Weapon(WeakShipDamage))
                .Build();
    }
}