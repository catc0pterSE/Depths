using UnityEngine;

namespace ECS.Scripts.Data
{
    [CreateAssetMenu(menuName = "Create StaticData", fileName = "StaticData", order = 0)]
    public sealed class StaticData : ScriptableObject
    {
        [field: SerializeField] public GameObject UnitPrefab { get; private set; }
        [field: SerializeField] public GameObject ItemPrefab { get; private set; }
        [field: SerializeField] public BodyPartData[] BodyPartsData { get; private set; }
        [field: SerializeField] public StatData[] StatsData { get; private set; }
    }
}