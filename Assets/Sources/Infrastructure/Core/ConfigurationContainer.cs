using UnityEngine;

namespace Sources.Infrastructure.Core
{
    [CreateAssetMenu(menuName = "Data/Create ConfigurationContainer", fileName = "ConfigurationContainer", order = 0)]
    public class ConfigurationContainer : ScriptableObject
    {
        [field: SerializeField] public Vector3 MiddleSpawnPoint { get; private set; }
        [field: SerializeField] public float VerticalPositionStep { get; private set; }
        [field: SerializeField] public float HorizontalPositionStep { get; private set; }
        [field: SerializeField] public GameObject EnemyShipPrefab { get; private set; }
        [field: SerializeField] public GameObject PlayerShipPrefab { get; private set; }
        [field: SerializeField] public Vector3 PlayerSpawnPosition { get; private set; }
        [field: SerializeField] public int MaxDistanceFromSpawn { get; private set; }
        [field: SerializeField] public float SpawnInterval { get; private set; }
    }
}