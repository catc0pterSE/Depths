using UnityEngine;

namespace ECS.Scripts.Path.Component
{
    public struct CreatePath
    {
        public Vector3 start;
        public Vector3 end;
    }
    
    public struct TargetDrop
    {
        public Vector3 value;
    }
}