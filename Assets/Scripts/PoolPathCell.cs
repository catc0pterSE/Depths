using DefaultNamespace;
using UnityEngine;

public class PoolPathCell : MonoBehaviour
{
    private const int StartCount = 5;
        
    [SerializeField] private CellView _prefab;

    private PoolMono<CellView> _pool;

    private void Awake() => 
        _pool = new PoolMono<CellView>(_prefab, StartCount, transform, new Factory<CellView>());

    public CellView GetFreeCell() => 
        _pool.GetFreeElement();
}