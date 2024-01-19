using Unity.Mathematics;
using UnityEngine;

namespace ECS.Scripts.Path.Systems
{
    public static class float3Extensions
    {
        public static float3 ToFloat3(this Vector3 input)
        {
            return new float3( input.x, input.y, input.z  );
        }
        public static float3 MoveTowards(this float3 current, float3 target, float maxDistanceDelta)
        {
            float deltaX = target.x - current.x;
            float deltaY = target.y - current.y;
            float deltaZ = target.z - current.z;

            float sqdist = deltaX * deltaX + deltaY * deltaY  + deltaZ * deltaZ ;

            if (sqdist == 0 || sqdist <= maxDistanceDelta * maxDistanceDelta)
                return target;
            var dist = math.sqrt(sqdist);

            return new float3(current.x + deltaX / dist * maxDistanceDelta,
                current.y + deltaY / dist * maxDistanceDelta,
                current.z + deltaZ / dist * maxDistanceDelta);
        }
    }
}