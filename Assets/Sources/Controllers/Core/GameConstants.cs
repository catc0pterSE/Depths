using UnityEngine;

namespace Sources.Controllers.Core
{
    internal static class GameConstants
    {
        public static int MaxCollisionPerShip = 5;
        public static Vector2 PlayerMoveHorizontalConstrain = new Vector2(-2, 2);
        public static int MaxDistanceFromSpawn = 150;
        public static Vector3 EnemySpawnRotation = new Vector3(0, 0, 180);
    }
}