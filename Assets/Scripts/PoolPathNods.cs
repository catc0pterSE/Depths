using UnityEngine;

public class PoolPathNods : MonoBehaviour
{
    private const int Count = 5;
        
    [SerializeField] private PathNode _prefab;

    private PoolMono<PathNode> _pool;

    private void Awake() => 
        _pool = new PoolMono<PathNode>(_prefab, Count, transform);

    public PathNode GetFreeNode() => 
        _pool.GetFreeElement();
}