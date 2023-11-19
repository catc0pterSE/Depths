using UnityEngine;

namespace Sources.Infrastructure.Api.Services.Providers
{
    public interface IConfigurationProvider
    {
        GameObject PlayerShipPrefab { get; }
        GameObject EnemyShipPrefab { get; }
        Vector3 PlayerSpawnPosition { get; }
        int MaxDistanceFromSpawn { get; }
        float SpawnInterval { get; }
        float HorizontalPositionStep { get; }
        Vector3 MiddleSpawnPoint { get; }
    }
}