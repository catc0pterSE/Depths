using System.Collections;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Boot
{
	public sealed class Game : MonoBehaviour
    {
	    private EcsWorld _world;
	    private EcsSystems _systems;
        
        [SerializeField] private SceneData _sceneData;
		[SerializeField] private RuntimeData _runtimeData;
		[SerializeField] private StaticData _staticData;
		[SerializeField] private LevelPN _levelPn;

        IEnumerator Start()
        {
            // void can be switched to IEnumerator for support coroutines.

            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            _runtimeData = new RuntimeData();
            
            _systems
		            
	            .Add(new SpawnUnitSystem())
	            .Add(new CreatePathSystem())
	            .Add(new SetNotWalkingSystem())
	            .Add(new PathMoveSystem())
	            .Add(new UpdatePositionSystem())
	            
	            .Inject(_sceneData)
	            .Inject(_runtimeData)
	            .Inject(_staticData)
	            .Inject(_levelPn)

	            .Init();

            yield return null;
        }
        

        void Update()
        {
	        _runtimeData.deltaTime = Time.deltaTime;
            _systems?.Run();
        }

        void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
                _world.Destroy();
                _world = null;
            }
        }
    }
}