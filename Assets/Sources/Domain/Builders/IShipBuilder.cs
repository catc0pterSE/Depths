using Sources.Domain.Models.Enginies;
using Sources.Domain.Models.Hulls;
using Sources.Domain.Models.Ships;
using Sources.Domain.Models.Weapons;

namespace Sources.Domain.Builders
{
    public interface IShipBuilder
    {
        IShipBuilder SetHull(IHull hull);
        IShipBuilder SetWeapon(IWeapon weapon);
        IShipBuilder SetEngine(IEngine engine);
        void Clear();
        IShip Build();
    }
}