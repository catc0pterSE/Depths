using System.Collections.Generic;
using DefaultNamespace;
using ECS.Scripts.CharacterComponent;
using ECS.Scripts.Data;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using ECS.Scripts.ProviderComponents;
using ECS.Scripts.TestSystem;
using ECS.Scripts.WorkFeature;
using Leopotam.EcsProto;
using Leopotam.EcsProto.Ai.Utility;
using Leopotam.EcsProto.QoL;
using Level;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ECS.Scripts.Boot
{
    public sealed class SelectionAspect : ProtoAspectInject
    {
        public ProtoPool<CanSelect> CanSelect;

        public ProtoPool<Selected> Selected;

        public ProtoPool<SelectedView> SelectedView;

        public ProtoPool<SelectedEvent> SelectedEvent;

        public readonly ProtoItExc SelectedUnitsEvent = new(It.Inc<SelectedEvent>(), It.Exc<Selected>());

        public readonly ProtoIt ItemsEvent = new(It.Inc<Item, SelectedEvent>());
        public readonly ProtoIt MiningEvent = new(It.Inc<MiningTag, SelectedEvent>());
        
        public readonly ProtoItExc SelectedUnitViewsOn = new(It.Inc<SelectedEvent, TransformRef>(), It.Exc<Selected>());

        public readonly ProtoIt SelectedViewIt = new(It.Inc<SelectedView>());
        
        public readonly ProtoIt SelectedItemsIt = new(It.Inc<Item, Selected>());
        public readonly ProtoIt SelectedMiningIt = new(It.Inc<MiningTag, Selected>());

        public readonly ProtoIt SelectedIt = new(It.Inc<Selected>());
        
    }


    public struct ZoneMode
    {
    }

    public struct Zone
    {
        public Dictionary<Vector2Int, GameObject> Cells;
    }
    public sealed class ZoneAspect : ProtoAspectInject
    {
        public readonly ProtoPool<ZoneMode> ZoneMode;
        
        public readonly ProtoPool<Zone> Zone;
        
        public readonly ProtoIt ZoneIt = new(It.Inc<Zone>());
        
        public readonly ProtoIt ZoneModeIt = new(It.Inc<ZoneMode>());
    }

    public sealed class ZoneSystem : IProtoRunSystem
    {
        [DI] private readonly ZoneAspect _zoneAspect;
        [DI] private readonly PathFindingService _path;
        [DI] private readonly MainAspect _mainAspect;
        [DI] private readonly StaticData _staticData;
        
        private bool _startSelected;
        private Vector3 _startPos;
        private Vector3 _endPos;
        public void Run()
        {
            if(_zoneAspect.ZoneMode.Len() == 0) return;
 
            if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
            {
                foreach (var protoEntity in _zoneAspect.ZoneModeIt)
                {
                    _mainAspect.ZoneAspect.ZoneMode.Del(protoEntity);
                }
                
                _startSelected = false;
                return;
            }
            
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                _startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _startSelected = true;
            }

            if (Input.GetMouseButton(0) && _startSelected)
            {
                _endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            
            if (Input.GetMouseButtonUp(0) && _startSelected)
            {
                
                var startPos = _path.Grid.ClampPosition(_startPos).CeilPositionInt3();
                var endPos = _path.Grid.ClampPosition(_endPos).CeilPositionInt3();
                
                int startX;
                int endX;
                
                if (_startPos.x < _endPos.x)
                {
                    startX = startPos.x;
                    endX = endPos.x;
                }
                else
                {
                    startX =  endPos.x;
                    endX = startPos.x;
                }
                
                int startY;
                int endY;
                
                if (_startPos.y < _endPos.y)
                {
                    startY = startPos.y;
                    endY = endPos.y;
                }
                else
                {
                    startY =  endPos.y;
                    endY = startPos.y;
                }

                var world = _mainAspect.World();
                var newZoneEntity = world.NewEntity();
                ref var zone = ref _mainAspect.ZoneAspect.Zone.Add(newZoneEntity);
                zone.Cells = new Dictionary<Vector2Int, GameObject>();
                
                for (int x = startX ; x < endX; x++)
                {
                    for (int y = startY; y < endY; y++)
                    {
                        var posInt = new Vector2Int(x,y);
          
                        var zoneCell = Object.Instantiate(_staticData.ZoneCell);
                        zone.Cells.Add(posInt, zoneCell.gameObject);
                        
                        zoneCell.transform.position = new Vector3(x, y, 0);

                    }
                }
                
                _startSelected = false;
            }
        }
    }

    public sealed class MainAspect : ProtoAspectInject
    {
        public readonly PathAspect PathAspect;
        public readonly BodyAspect BodyAspect;
        public readonly StatAspect StatAspect;
        public readonly SelectionAspect SelectionAspect;
        public readonly ZoneAspect ZoneAspect;
        public readonly AiUtilityModuleAspect AiUtilityAspect;

        // public readonly ProtoPool<CreateBuild> CreateBuild;
        // public readonly ProtoIt CreateBuildIt = new(It.Inc<CreateBuild>());


        public bool AddSolution<T>(ProtoPool<T> pool, ProtoEntity entity, out ProtoEntity solutionEntity) where T : struct
        {
            ref var aiSolution = ref AISolution.Get(entity);
            var protoWorld = World();

            if (aiSolution.packedEntity.Unpack(protoWorld, out var unPackedSolutionEntity))
            {
                solutionEntity = unPackedSolutionEntity;
                
                if(pool.Has(unPackedSolutionEntity)) return false;
                
                protoWorld.DelEntity(unPackedSolutionEntity);
            }
            
            var solutionNewEntity = protoWorld.NewEntity();
            
            pool.Add(solutionNewEntity);
            
            Owners.Add(solutionNewEntity).value = protoWorld.PackEntity(entity);
            
            aiSolution.packedEntity = protoWorld.PackEntity(solutionNewEntity);

            solutionEntity = solutionNewEntity;
            
            return true;
        }

        public ProtoPool<AddCell> AddCell;
        public readonly ProtoIt AddCellIt = new(It.Inc<AddCell, Position>());
        
        public ProtoPool<Owner> Owners;
        public ProtoPool<BuildWall> Build;
        public ProtoPool<BuildTag> BuildTeg;
        
        public ProtoPool<Unit> Units;

        public ProtoPool<RandMove> RandMove;

        public readonly ProtoPool<Direction> Direction;

        public readonly ProtoPool<TransformRef> Transforms;

        public readonly ProtoPool<Position> Position;

        public readonly ProtoPool<Health> Health;

        public readonly ProtoPool<OnChangeHealth> OnChangeHealth;

        public readonly ProtoPool<Speed> Speed;

        public readonly ProtoPool<Works> Works;
        
        public readonly ProtoPool<FindNearElement> FindNearElement;
        
        public readonly ProtoPool<TargetWork> TargetWork;
        
        public readonly ProtoIt FindNearElementIt =new(It.Inc<FindNearElement>());
        
        public readonly ProtoIt TargetWorkIt =new(It.Inc<TargetWork>());


        // to do -> 
        public readonly ProtoPool<FindWork> FindWork;

        public readonly ProtoPool<DiedEvent> DiedsEvent;

        public readonly ProtoPool<WorkProcess> WorkProcess;
        public readonly ProtoIt WorkProcessIt = new(It.Inc<WorkProcess, Owner>());

        public readonly ProtoPool<CancelWork> CancelWork;

        public readonly ProtoIt CancelWorkF = new(It.Inc<CancelWork>());

        public readonly ProtoPool<ItemBusy> ItemsBusy;
        
        
        public readonly ProtoIt AISolutionIt = new(It.Inc<AISolution>());
        
        public readonly ProtoIt AISolutionResponceIt = new(It.Inc<AISolution, AiUtilityResponseEvent>());
        
        public readonly ProtoPool<AISolution> AISolution;
        
        public readonly ProtoPool<FindFood> FindFood;


        public readonly ProtoItExc SyncPosition = new(It.Inc<TransformRef, Position>(), It.Exc<Sync>());

        //public readonly ProtoIt MoveDirection = new(It.Inc<Position, Direction, Speed>());

        public readonly ProtoIt MoveDirection = new(It.Inc<Position, Direction, Speed>());

        public readonly ProtoIt UnitsSelected = new(It.Inc<Unit, Selected>());
        public readonly ProtoIt BuildFilter = new(It.Inc<BuildWall>());

        public readonly ProtoIt RandsMover = new(It.Inc<RandMove, Owner>());


        public readonly ProtoPool<Sync> Sync;

        public readonly ProtoPool<MiningTag> MiningTag;
        public readonly ProtoPool<MineProcess> MineProcess;
        public readonly ProtoPool<Mining> Mining;


        public readonly ProtoItExc MiningFree = new(It.Inc<MiningTag, MarkerWork, Position>(), It.Exc<ItemBusy>());

        public readonly ProtoIt MiningDied = new(It.Inc<MiningTag, Health, Position>());
        

        public readonly ProtoItExc MiningProcessMove = new(It.Inc<MineProcess, TargetWork>(), It.Exc<Mining>());

        public readonly ProtoIt MiningProcessMining = new(It.Inc<MineProcess, TargetWork, Mining>());

        public readonly ProtoIt MiningProcessCancel = new(It.Inc<MineProcess, TargetWork, CancelWork>());


        public readonly ProtoPool<FindItemProcess> FindItemProcess;

        public readonly ProtoPool<ItemWork> ItemWork;

        public readonly ProtoPool<ItemInHand> ItemsInHand;
        
        public readonly ProtoPool<Drop> Drop;
        
        public readonly ProtoIt DropItemIt = new(It.Inc<ItemInHand, Drop>());

        public readonly ProtoPool<Item> Items;

        public readonly ProtoPool<TargetDrop> TargetDrop;
        
        public readonly ProtoPool<CurrentWork> CurrentWork;
        
        public readonly ProtoPool<NewWork> NewWork;
        
        public readonly ProtoPool<WorkCost> WorkCost;
        
        
        public readonly ProtoPool<MarkerWork> MarkerWork;

        public readonly ProtoItExc ItemsFree = new(It.Inc<Item, MarkerWork, Position>(), It.Exc<ItemBusy>());
        public readonly ProtoIt ItemsLive = new(It.Inc<Item, MarkerWork, Position>());
        
        public readonly ProtoIt NewWorkIt = new(It.Inc<NewWork>());
        

        public readonly ProtoIt FindItemProcessGet =
            new(It.Inc<FindItemProcess>());
        
        public readonly ProtoIt FindZoneForItemIt = 
            new(It.Inc<FindItemProcess>());

        public readonly ProtoIt FindItemProcessDrop = 
            new(It.Inc<FindItemProcess, TargetDrop>());
        public readonly ProtoIt FindItemProcessCancel = 
            new(It.Inc<FindItemProcess, TargetWork, CancelWork>());


        public readonly ProtoItExc WorkersNotWorking = new(It.Inc<Works>(),
            It.Exc<CurrentWork>());

        public readonly ProtoItExc FindWorkF = new(It.Inc<FindWork, Position>(),
            It.Exc<WorkProcess, PathFeature.Components.EntityPath>());
        
    }
}