using System.Collections;
using ECS.Scripts.Data;
using ECS.Scripts.PathFeature.Systems;
using ECS.Scripts.SyncSystems;
using ECS.Scripts.TestSystem;
using ECS.Scripts.WorkFeature;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Level;
using UnityEngine;
using UnityEngine.Serialization;

namespace ECS.Scripts.Boot
{
	public sealed class Game : MonoBehaviour
    {
	    private ProtoWorld _world;
	    private ProtoSystems _systems;
	    
        [SerializeField] private SceneData _sceneData;
		[SerializeField] private RuntimeData _runtimeData;
		[SerializeField] private StaticData _staticData;
		[SerializeField] private SpatialHash _spatialHash;
		[FormerlySerializedAs("_levelPn")] [SerializeField] private PathFindingService pathFindingService;
		
		[SerializeField] private UnitWindow _window;
		[SerializeField] private SelectionView _selectionView;

        IEnumerator Start()
        {
            // void can be switched to IEnumerator for support coroutines.

            _world = new ProtoWorld(new MainAspect());
            _systems = new ProtoSystems(_world);

            _spatialHash = new SpatialHash(pathFindingService.Grid);

// #if UNITY_EDITOR
//             Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
//             Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
// #endif
            
            _runtimeData = new RuntimeData();


            ISelectionService selectionService = new SelectionService();
            UnitPresenter presenter = new UnitPresenter(selectionService, _window);
            _systems
		            
	            .AddModule(new AutoInjectModule())   
	            .AddSystem(new SpawnItemSystem())
	            .AddSystem(new SpawnUnitSystem())
	            //.AddSystem(new SetNotWalkingSystem())
	            
	            .AddSystem(new CreateMousePathSystem())
	            .AddSystem(new CreateRandPathSystem())
	            
	            .AddSystem(new SelectionSystem())
	            .AddSystem(new SelectedViewEventSystem())
	            .AddSystem(new SelectedEventSystem())
	            
	            // start work
                
	            .AddSystem(new WorkSystem())
	            
	            .AddSystem(new FindItemProcessSystem())
	            .AddSystem(new MineProcessSystem())
	            
	            .AddSystem(new WorkCancelSystem())
	            
	            // end work
                
	            
	            .AddSystem(new CreatePathSystem())
	            
	            
	            // Dieds
	            .AddSystem(new MineDiedSystem())
	            
	            
	            .AddSystem(new CheckDistancePointInPathSystem())
	            .AddSystem(new PathMoveSystem())
	            .AddSystem(new UpdatePositionSystem())
	            
	            .AddService(_sceneData)
	            .AddService(_runtimeData)
	            .AddService(_staticData)
	            .AddService(_spatialHash)
	            .AddService(_selectionView)
	            .AddService(pathFindingService)

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