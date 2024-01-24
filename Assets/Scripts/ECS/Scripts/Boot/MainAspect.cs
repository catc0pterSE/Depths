using ECS.Scripts.CharacterComponent;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using ECS.Scripts.ProviderComponents;
using ECS.Scripts.TestSystem;
using ECS.Scripts.WorkFeature;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.Boot
{

	public sealed class SelectionAspect : ProtoAspectInject
	{
		public ProtoPool<CanSelect> CanSelect;
        
		public ProtoPool<Selected> Selected;
        
		public ProtoPool<SelectedEvent> SelectedEvent;
        
		public readonly ProtoIt SelectedEventIt = new (It.Inc<SelectedEvent>());
        
		public readonly ProtoIt SelectedIt = new (It.Inc<Selected>());
	}
    public sealed class MainAspect : ProtoAspectInject
    {
        public readonly PathAspect PathAspect;
        public readonly BodyAspect BodyAspect;
        public readonly StatAspect StatAspect;
        public readonly SelectionAspect SelectionAspect;
		
        // public readonly ProtoPool<CreateBuild> CreateBuild;
        // public readonly ProtoIt CreateBuildIt = new(It.Inc<CreateBuild>());
        
        
        public ProtoPool<Owner> Owners;
		
        public ProtoPool<Unit> Units;
		
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
		
        public readonly ProtoItExc RandsMover = new (It.Inc<Position, RandMove>(), It.Exc<PathFeature.Components.EntityPath, WorkProcess>());
		
		
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
		
		
		
		
        public readonly ProtoItExc WorkersNotWorking = new (It.Inc<Works, Position>(), It.Exc<WorkProcess, PathFeature.Components.EntityPath>());
        public readonly ProtoItExc FindWorkF = new (It.Inc<FindWork, Position>(), It.Exc<WorkProcess, PathFeature.Components.EntityPath>());
		
		
    }
}