using UnityEngine;

namespace ECS.Boot
{
    [CreateAssetMenu(menuName = "Create Stat", fileName = "StatData", order = 0)]
    public sealed class StatData : ScriptableObject
    {
        [field: SerializeField] public StatType Stat { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}