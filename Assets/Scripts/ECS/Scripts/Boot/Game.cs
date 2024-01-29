using System;
using System.Collections;
using System.Collections.Generic;
using ECS.Scripts.Data;
using ECS.Scripts.PathFeature.Systems;
using ECS.Scripts.SyncSystems;
using ECS.Scripts.TestSystem;
using ECS.Scripts.WorkFeature;
using Leopotam.EcsProto;
using Leopotam.EcsProto.Ai.Utility;
using Leopotam.EcsProto.QoL;
using Level;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace ECS.Scripts.Boot
{
	public sealed class Game : SerializedMonoBehaviour
    {
	    private ProtoWorld _world;
	    private ProtoSystems _systems;
	    
        [SerializeField] private SceneData _sceneData;
		[SerializeField] private RuntimeData _runtimeData;
		[SerializeField] private StaticData _staticData;
		[SerializeField] private SpatialHash _spatialHash;
		[SerializeField] private PathFindingService pathFindingService;
		
		[SerializeField] private UnitWindow _window;
		[SerializeField] private SelectionView _selectionView;

		[SerializeField] private ButtonBuild _button;
		
		IEnumerator Start()
        {
            // void can be switched to IEnumerator for support coroutines.

            var mainAspect = new MainAspect();
            
            _button.Construct(mainAspect);
            _world = new ProtoWorld(mainAspect);
            _systems = new ProtoSystems(_world);

            _spatialHash = new SpatialHash(pathFindingService.Grid);

// #if UNITY_EDITOR
//             Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
//             Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
// #endif
            
            _runtimeData = new RuntimeData();


            _selectionView.Construct(mainAspect);

            var aiModule = new AiUtilityModule(default, "utility-ai", new IAiUtilitySolver[]
            {
	            new BeginCompleteWorkSolver(),
	            new RandSolver()
            });
            
            
            ISelectionService selectionService = new SelectionService();
            UnitPresenter presenter = new UnitPresenter(selectionService, _window);
            _systems
		            
	            .AddModule(new AutoInjectModule())   
	            .AddModule(aiModule)
	            
	            .AddPoint ("utility-ai")
	            
	            .AddSystem(new SpawnItemSystem())
	            .AddSystem(new SpawnUnitSystem())
	            //.AddSystem(new SetNotWalkingSystem())
	            
	            .AddSystem(new CreateMousePathSystem())
	            .AddSystem(new CreateRandPathSystem())
	            
	            .AddSystem(new ZoneSystem())
	            .AddSystem(new SelectionSystem())
	            .AddSystem(new SelectedViewEventSystem())
	            .AddSystem(new SelectedEventSystem())
	            
	            .AddSystem(new BuildSystem())
	            // start work
                
	            .AddSystem(new UtilityApplySystem())
	            
	            .AddSystem(new NewWorkSystem())
	            
	            .AddSystem(new FindNearElementSystem())
	            
	            .AddSystem(new WorkNotFindElementSystem())
	            
	            .DelHere<FindNearElement>()
	            
	            //.AddSystem(new WorkSystem())
	            
	            .AddSystem(new FindItemProcessSystem())
	            
	            .AddSystem(new MineProcessSystem())
	            
	            .AddSystem(new WorkCancelSystem())
	            
	            .AddSystem(new UtilityCreateRequestSystem())
	            
	            .AddSystem(new DropItem())
	            
	            .AddSystem(new AddCellSystem())
	            
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