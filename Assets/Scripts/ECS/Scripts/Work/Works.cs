using Leopotam.Ecs;

namespace ECS.Scripts.Work
{
    public struct Works
    {
        public Work[] value;
    }

    public struct ItemPlaced
    {
    }
    public struct ItemInHand
    {
        public EcsEntity value;
    }
    public struct Sync
    {
    }
}