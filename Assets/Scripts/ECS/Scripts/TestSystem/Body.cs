using System.Collections.Generic;
using Leopotam.Ecs;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.TestSystem
{
    public struct Body
    {
        public Dictionary<BodyPart, ProtoPackedEntity> parts;
    }
}