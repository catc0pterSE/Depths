using Sources.Domain.Models.Ships;

namespace Sources.Domain.Factories
{
    public interface IShipFactory
    {
        IShip CreateDefault();
        IShip CreateWeak();
    }
}