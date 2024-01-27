using UnityEngine;

namespace Grid.Quad
{
    public class Point
    {
        public Point(Vector2Int size) => 
            Size = size;

        public Vector2Int Size { get; private set; }
    }
}