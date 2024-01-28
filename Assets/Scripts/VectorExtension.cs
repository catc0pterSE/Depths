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
        
        public static Vector2Int FloorPosition(this Vector3 position) => 
            new(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
        public static Vector2Int RoundPosition(this Vector3 position) => 
            new(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        
        public static Vector2Int CeilPositionInt2(this Vector3 position) => 
            new(Mathf.CeilToInt(position.x), Mathf.CeilToInt(position.y));
        public static Vector3Int CeilPositionInt3(this Vector3 position) => 
            new(Mathf.CeilToInt(position.x), Mathf.CeilToInt(position.y));
        public static Vector3 CeilPosition(this Vector3 position) => 
            new(Mathf.Ceil(position.x), Mathf.Ceil(position.y));
    }
}