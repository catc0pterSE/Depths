using ECS.Scripts.TestSystem;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.Boot
{
    public sealed class StatAspect : ProtoAspectInject
    {
        public readonly ProtoPool<Stats> Stats;
		
        public readonly ProtoPool<Stat> Stat;
		
    }
}