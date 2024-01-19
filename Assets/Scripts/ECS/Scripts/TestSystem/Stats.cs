using System.Collections.Generic;
using Leopotam.Ecs;

namespace ECS.Scripts.TestSystem
{
    public struct Stats
    {
        public Dictionary<StatType, EcsEntity> value;
    }
}