using ECS.Scripts.TestSystem;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.Boot
{
    public sealed class BodyAspect : ProtoAspectInject
    {
        public readonly ProtoPool<Head> Heads;
		
        public readonly ProtoPool<Part> Parts;
		
        public readonly ProtoPool<Body> Bodies;
		
        public readonly ProtoIt HeadsChangeHealth = new(It.Inc<Head, Health, OnChangeHealth>());
		
        public readonly ProtoIt PartsChangeHealth = new(It.Inc<Part, Health, OnChangeHealth>());
    }
}