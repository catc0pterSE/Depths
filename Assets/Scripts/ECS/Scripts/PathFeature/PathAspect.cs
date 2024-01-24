using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using ECS.Scripts.PathFeature.Components;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.Boot
{
    public sealed class PathAspect : ProtoAspectInject
    {
        public readonly ProtoPool<EntityPath> Path;
		
        public readonly ProtoPool<UpdatePath> UpdatePath;
		
        public readonly ProtoPool<CreatePath> CreatePath;
		
        public readonly ProtoIt CreatePathIt = new(It.Inc<CreatePath>());
		
        public readonly ProtoIt PathUpdate = new(It.Inc<Position, EntityPath, UpdatePath>());
		
        public readonly ProtoIt MovePath = new(It.Inc<Position, EntityPath>());
    }
}