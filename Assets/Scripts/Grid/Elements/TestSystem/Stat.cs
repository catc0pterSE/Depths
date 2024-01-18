using System.Runtime.CompilerServices;

namespace ECS.Scripts.TestSystem
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