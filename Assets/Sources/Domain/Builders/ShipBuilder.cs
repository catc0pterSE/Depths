using Sources.Domain.Models.Enginies;
using Sources.Domain.Models.Hulls;
using Sources.Domain.Models.Ships;
using Sources.Domain.Models.Weapons;

namespace Sources.Domain.Builders
{
    public class ShipBuilder : IShipBuilder
    {
        private IHull _hull;
        private IWeapon _weapon;
        private IEngine _engine;

        public ShipBuilder()
        {
            Clear();
        }

        public IShipBuilder SetHull(IHull hull)
        {
            _hull = hull;
            return this;
        }

        public IShipBuilder SetWeapon(IWeapon weapon)
        {
            _weapon = weapon;
            return this;
        }

        public IShipBuilder SetEngine(IEngine engine)
        {
            _engine = engine;
            return this;
        }

        public void Clear()
        {
            _hull = null;
            _weapon = null;
            _engine = null;
        }

        public IShip Build() => 
            new Ship(_hull, _weapon, _engine);
    }
}