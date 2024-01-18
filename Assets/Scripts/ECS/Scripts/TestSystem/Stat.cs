using System.Runtime.CompilerServices;

namespace ECS.Boot
{
    public struct Stat
    {
        public StatType type;
        public float value;
        public float bonusValue;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float TotalValue() => value + bonusValue;
    }
}