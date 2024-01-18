using System.Collections.Generic;
using Leopotam.Ecs;

namespace ECS.Boot
{
    public struct Body
    {
        public Dictionary<BodyPart, EcsEntity> parts;
    }
}