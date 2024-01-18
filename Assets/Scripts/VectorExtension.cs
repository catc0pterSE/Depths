using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    public static class VectorExtension
    {
        public static Vector3Int ToInt(this Vector3 position) => 
            new((int)position.x, (int)position.y, 0);
        
        public static Vector3Int ToVector3(this Vector2Int position) => 
            new(position.x, position.y, 0);
    }
}