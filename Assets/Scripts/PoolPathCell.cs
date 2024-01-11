using DefaultNamespace;
using UnityEngine;

public class PoolPathCell : MonoBehaviour
{
    private const int StartCount = 5;
        
    [SerializeField] private Cell _prefab;

    private PoolMono<Cell> _pool;

    private void Awake() => 
        _pool = new PoolMono<Cell>(_prefab, StartCount, transform, new Factory<Cell>());

    public Cell GetFreeCell() => 
        _pool.GetFreeElement();
}