using System.Collections.Generic;
using Leopotam.Ecs;

namespace ECS.Scripts.TestSystem
{
    public struct Body
    {
        public Dictionary<BodyPart, EcsEntity> parts;
    }
}