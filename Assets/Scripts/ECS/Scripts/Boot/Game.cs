using System.Collections;
using ECS.Scripts.Data;
using ECS.Scripts.Path.Systems;
using ECS.Scripts.SyncSystems;
using ECS.Scripts.TestSystem;
using ECS.Scripts.Work;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Scripts.Boot
{
	public sealed class Game : MonoBehaviour
    {
	    private EcsWorld _world;
	    private EcsSystems _systems;
	    
        [SerializeField] private SceneData _sceneData;
		[SerializeField] private RuntimeData _runtimeData;
		[SerializeField] private StaticData _staticData;
		[SerializeField] private LevelPN _levelPn;
		
		[SerializeField] private UnitWindow _window;

        IEnumerator Start()
        {
            // void can be switched to IEnumerator for support coroutines.

            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            
            _runtimeData = new RuntimeData();


            ISelectionService selectionService = new SelectionService();
            UnitPresenter presenter = new UnitPresenter(selectionService, _window);
            _systems
		            
	            .Add(new SpawnUnitSystem())
	            .Add(new SetNotWalkingSystem())
	            
	            .Add(new CreateMousePathSystem())
	            .Add(new CreateRandPathSystem())
	            
	            .Add(new CreatePathSystem())
	            
	            .Add(new WorkSystem())
	            
	            .Add(new FindItemProcessSystem())
	            
	            .Add(new WorkCancelSystem())
	            
	            
	            .Add(new PathMoveSystem())
	            .Add(new UpdatePositionSystem())
	            
	            .Inject(_sceneData)
	            .Inject(_runtimeData)
	            .Inject(_staticData)
	            .Inject(selectionService)
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