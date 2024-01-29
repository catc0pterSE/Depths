using ECS.Scripts.TestSystem;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.Boot
{
    public sealed class StatAspect : ProtoAspectInject
    {
        public readonly ProtoIt StatsIt = new(It.Inc<Stats>());
        public readonly ProtoPool<Stats> Stats;
		
        public readonly ProtoIt StatIt = new(It.Inc<Stat>());
        public readonly ProtoPool<Stat> Stat;
    }
}