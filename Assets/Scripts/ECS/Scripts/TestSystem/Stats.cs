using System.Collections.Generic;
using Leopotam.Ecs;

namespace ECS.Boot
{
    public struct Stats
    {
        public Dictionary<StatType, EcsEntity> value;
    }
}