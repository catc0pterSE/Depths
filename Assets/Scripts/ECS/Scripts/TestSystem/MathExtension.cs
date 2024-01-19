using UnityEngine;

namespace ECS.Scripts.TestSystem
{
    public static class MathExtension
    {
        public static void MinDistance(this float dist, Vector3 pointA, Vector3 pointB)
        {
            var distanceTwo = (pointA - pointB).sqrMagnitude;

            if (dist < distanceTwo)
            {
                dist = distanceTwo;
            }
        }
        public static float FastDistance(this Vector3 pointA, Vector3 pointB)
        {
            return (pointA - pointB).sqrMagnitude;
        }
    }
}