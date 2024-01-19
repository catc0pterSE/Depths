using ECS.Scripts.TestSystem;
using UnityEngine;

namespace ECS.Scripts.Data
{
    [CreateAssetMenu(menuName = "Create BodyPart", fileName = "BodyPart", order = 0)]
    public sealed class BodyPartData : ScriptableObject
    {
        [field: SerializeField] public BodyPart Part { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}