using System.Collections.Generic;
using Leopotam.Ecs;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.TestSystem
{
    public struct Stats
    {
        public Dictionary<StatType, ProtoPackedEntity> value;
    }
}