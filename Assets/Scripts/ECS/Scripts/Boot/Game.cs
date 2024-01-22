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
	public sealed class PathAspect : ProtoAspectInject
	{
		public readonly ProtoPool<PathFeature.Components.Path> Path;
		
		public readonly ProtoPool<UpdatePath> UpdatePath;
		
		public readonly ProtoPool<TargetPoint> TargetPoint;
		
		public readonly ProtoIt PathCreate = new(It.Inc<Position, TargetPoint>());
		
		public readonly ProtoIt PathUpdate = new(It.Inc<Position, PathFeature.Components.Path, UpdatePath>());
		
		public readonly ProtoIt MovePath = new(It.Inc<Position, PathFeature.Components.Path>());
	}
	public sealed class StatAspect : ProtoAspectInject
	{
		public readonly ProtoPool<Stats> Stats;
		
		public readonly ProtoPool<Stat> Stat;
		
	} 
	public sealed class BodyAspect : ProtoAspectInject
	{
		public readonly ProtoPool<Head> Heads;
		
		public readonly ProtoPool<Part> Parts;
		
		public readonly ProtoPool<Body> Bodies;
		
		public readonly ProtoIt HeadsChangeHealth = new(It.Inc<Head, Health, OnChangeHealth>());
		
		public readonly ProtoIt PartsChangeHealth = new(It.Inc<Part, Health, OnChangeHealth>());
	}
	
	public sealed class MainAspect : ProtoAspectInject
	{
		public readonly PathAspect PathAspect;
		public readonly BodyAspect BodyAspect;
		public readonly StatAspect StatAspect;
		
		public ProtoPool<Owner> Owners;
		
		public ProtoPool<Unit> Units;
		
		public ProtoPool<Selected> Selected;
		
		public ProtoPool<RandMove> RandMove;
		
		
		public readonly ProtoPool<Direction> Direction;
		
		public readonly ProtoPool<TransformRef> Transforms;
		
		public readonly ProtoPool<Position> Position;
		
		public readonly ProtoPool<Health> Health;
		
		public readonly ProtoPool<OnChangeHealth> OnChangeHealth;
		
		public readonly ProtoPool<Speed> Speed;
		
		public readonly ProtoPool<Works> Works;
		
		
		// to do -> 
		public readonly ProtoPool<FindWork> FindWork;
		
		public readonly ProtoPool<DiedEvent> DiedsEvent;
		
		public readonly ProtoPool<WorkProcess> WorkProcess;
		
		public readonly ProtoPool<CancelWork> CancelWork;
		
		public readonly ProtoIt CancelWorkF = new(It.Inc<CancelWork>());
		
		public readonly ProtoPool<ItemBusy> ItemsBusy;
		
        
		public readonly ProtoItExc SyncPosition = new(It.Inc<TransformRef, Position>(), It.Exc<Sync>());
		
		//public readonly ProtoIt MoveDirection = new(It.Inc<Position, Direction, Speed>());
		
		public readonly ProtoIt MoveDirection = new (It.Inc<Position, Direction, Speed>());
		
		public readonly ProtoIt UnitsSelected = new(It.Inc<Unit>());
		
		public readonly ProtoItExc RandsMover = new (It.Inc<Position, RandMove>(), It.Exc<PathFeature.Components.Path, WorkProcess>());
		
		
		public readonly ProtoPool<Sync> Sync;
		
		public readonly ProtoPool<MiningTag> MiningTag;
		public readonly ProtoPool<MineProcess> MineProcess;
		public readonly ProtoPool<Mining> Mining;
		
		
		public readonly ProtoItExc MiningFree = new (It.Inc<MiningTag, Position>(), It.Exc<ItemBusy>());
		
		public readonly ProtoIt MiningDied= new (It.Inc<MiningTag, Health, Position>());
        
		public readonly ProtoItExc MiningProcessMove = new (It.Inc<MineProcess, Position>(), It.Exc<Mining>());
        
		public readonly ProtoIt MiningProcessMining = new (It.Inc<MineProcess, Position, Mining>());
		
		public readonly ProtoIt MiningProcessCancel = new (It.Inc<MineProcess, CancelWork>());

		
		public readonly ProtoPool<FindItemProcess> FindItemProcess;
		
		public readonly ProtoPool<ItemWork> ItemWork;
		
		public readonly ProtoPool<ItemInHand> ItemsInHand;
        
		public readonly ProtoPool<Item> Items;
        
		public readonly ProtoPool<TargetDrop> TargetDrop;
		
		public readonly ProtoItExc ItemsFree = new (It.Inc<Item, Position>(), It.Exc<ItemBusy>());
		
		public readonly ProtoItExc FindItemProcessGet = new (It.Inc<FindItemProcess, Position, TransformRef>(), It.Exc<ItemInHand>());
		public readonly ProtoIt FindItemProcessDrop = new (It.Inc<ItemInHand, Position, TargetDrop>());
		public readonly ProtoIt FindItemProcessCancel = new (It.Inc<FindItemProcess, CancelWork>());
		
		
		
		
		public readonly ProtoItExc WorkersNotWorking = new (It.Inc<Works, Position>(), It.Exc<WorkProcess, PathFeature.Components.Path>());
		public readonly ProtoItExc FindWorkF = new (It.Inc<FindWork, Position>(), It.Exc<WorkProcess, PathFeature.Components.Path>());
		
		
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