using System.Collections;
using ECS.Scripts.CharacterComponent;
using ECS.Scripts.Data;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using ECS.Scripts.PathFeature.Components;
using ECS.Scripts.PathFeature.Systems;
using ECS.Scripts.ProviderComponents;
using ECS.Scripts.SyncSystems;
using ECS.Scripts.TestSystem;
using ECS.Scripts.WorkFeature;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Level;
using UnityEngine;

namespace ECS.Scripts.Boot
{
	public sealed class MainAspect : ProtoAspectInject
	{
		public ProtoPool<Head> Heads;
		
		public ProtoPool<Owner> Owners;
		public ProtoPool<Unit> Units;
		public ProtoPool<Selected> Selected;
		public ProtoPool<RandMove> RandMove;
		
		public ProtoPool<Direction> Direction;
		public ProtoPool<TransformRef> Transforms;
		public ProtoPool<Position> Position;
		
		public ProtoPool<PathFeature.Components.Path> Path;
		public ProtoPool<UpdatePath> UpdatePath;
		public ProtoPool<TargetPath> TargetPath;
		
		public ProtoPool<Health> Health;
		public ProtoPool<OnChangeHealth> OnChangeHealth;
		
		public ProtoPool<Speed> Speed;
		
		public ProtoPool<Works> Works;
		
		public ProtoPool<Body> Bodies;
		public ProtoPool<Part> Parts;
		
		public ProtoPool<Stats> Stats;
		public ProtoPool<Stat> Stat;
		
		
		public ProtoPool<DiedEvent> DiedsEvent;
		
		
		public ProtoPool<WorkProcess> WorkProcess;
		
		
		public ProtoPool<CancelWork> CancelWork;
		
		public readonly ProtoIt CancelWorkF = new(It.Inc<CancelWork>());
		
		public ProtoPool<ItemBusy> ItemsBusy;
	
		
	
		public readonly ProtoIt HeadsChangeHealth = new(It.Inc<Head, Health, OnChangeHealth>());
		public readonly ProtoIt PartsChangeHealth = new(It.Inc<Part, Health, OnChangeHealth>());
        
		public readonly ProtoItExc SyncPosition = new(It.Inc<TransformRef, Position>(), It.Exc<Sync>());
		
		public readonly ProtoIt PathCreate = new(It.Inc<Position, TargetPath>());
		
		public readonly ProtoIt PathUpdate = new(It.Inc<Position, PathFeature.Components.Path, UpdatePath>());
		
		public readonly ProtoIt MovePath = new(It.Inc<Position, PathFeature.Components.Path>());
		
		public readonly ProtoIt MoveDirection = new(It.Inc<Position, Direction, Speed>());
		
		public readonly ProtoIt UnitsSelected = new(It.Inc<Unit>());
		
		public readonly ProtoItExc RandsMover = new (It.Inc<Position, RandMove>(), It.Exc<PathFeature.Components.Path, WorkProcess>());
		
        
		
		
		public ProtoPool<Sync> Sync;
		
		public ProtoPool<MiningTag> MiningTag;
		public ProtoPool<MineProcess> MineProcess;
		public ProtoPool<Mining> Mining;
		
		
		public readonly ProtoItExc MiningFree = new (It.Inc<MiningTag, Position>(), It.Exc<ItemBusy>());
		
		public readonly ProtoIt MiningDied= new (It.Inc<MiningTag, Health, Position>());
        
		public readonly ProtoItExc MiningProcessMove = new (It.Inc<MineProcess, Position>(), It.Exc<Mining>());
        
		public readonly ProtoIt MiningProcessMining = new (It.Inc<MineProcess, Position, Mining>());
		
		public readonly ProtoIt MiningProcessCancel = new (It.Inc<MineProcess, CancelWork>());

		
		public ProtoPool<FindItemProcess> FindItemProcess;
		
		public ProtoPool<ItemInHand> ItemsInHand;
        
		public ProtoPool<Item> Items;
        
		public ProtoPool<TargetDrop> TargetDrop;
		
		public readonly ProtoItExc ItemsFree = new (It.Inc<Item, Position>(), It.Exc<ItemBusy>());
		
		public readonly ProtoItExc FindItemProcessGet = new (It.Inc<FindItemProcess, Position, TransformRef>(), It.Exc<ItemInHand>());
		public readonly ProtoIt FindItemProcessDrop = new (It.Inc<ItemInHand, Position, TargetDrop>());
		public readonly ProtoIt FindItemProcessCancel = new (It.Inc<FindItemProcess, CancelWork>());
		
		
		
		
		public readonly ProtoItExc WorkersNotWorking = new (It.Inc<Works, Position>(), It.Exc<WorkProcess, PathFeature.Components.Path>());
		
		
	}
	
	
	public sealed class Game : MonoBehaviour
    {
	    private ProtoWorld _world;
	    private ProtoSystems _systems;
	    
        [SerializeField] private SceneData _sceneData;
		[SerializeField] private RuntimeData _runtimeData;
		[SerializeField] private StaticData _staticData;
		[SerializeField] private LevelPN _levelPn;
		
		[SerializeField] private UnitWindow _window;

        IEnumerator Start()
        {
            // void can be switched to IEnumerator for support coroutines.

            _world = new ProtoWorld(new MainAspect());
            _systems = new ProtoSystems(_world);

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
	            .AddService(_levelPn)

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