using Sources.Infrastructure.Api.Services.Providers;
using UnityEngine;

namespace Sources.Infrastructure.Core.Services.Providers
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly ConfigurationContainer _configurationContainer;

        public ConfigurationProvider(ConfigurationContainer configurationContainer)
        {
            _configurationContainer = configurationContainer;
        }

        public GameObject PlayerShipPrefab => _configurationContainer.PlayerShipPrefab;
        public GameObject EnemyShipPrefab => _configurationContainer.EnemyShipPrefab;
        public Vector3 PlayerSpawnPosition => _configurationContainer.PlayerSpawnPosition;
        public int MaxDistanceFromSpawn => _configurationContainer.MaxDistanceFromSpawn;
        public float SpawnInterval => _configurationContainer.SpawnInterval;
        public float HorizontalPositionStep => _configurationContainer.HorizontalPositionStep;
        public Vector3 MiddleSpawnPoint => _configurationContainer.MiddleSpawnPoint;
    }
}