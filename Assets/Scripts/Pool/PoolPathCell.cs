using Factory;
using Factory.Asset;
using Grid.Elements;
using Grid.Elements.Work.Cell;
using UnityEngine;

namespace Pool
{
    public class PoolPathCell : MonoBehaviour
    {
        private const int StartCount = 5;
        
        [SerializeField] private CellView _prefab;

        private PoolMono<CellView> _pool;

        private void Awake() => 
            _pool = new PoolMono<CellView>(StartCount, transform, new Factory<CellView>(new AssetProvider()));

        public CellView GetFreeCell() => 
            _pool.GetFreeElement();
    }
}