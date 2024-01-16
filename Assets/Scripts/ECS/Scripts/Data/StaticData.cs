using UnityEngine;

namespace ECS.Boot
{
    [CreateAssetMenu(menuName = "Create StaticData", fileName = "StaticData", order = 0)]
    public sealed class StaticData : ScriptableObject
    {
        [field: SerializeField] public GameObject UnitPrefab { get; private set; }
    }
}