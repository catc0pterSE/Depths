using UnityEngine;

namespace Grid.Quad
{
    public class CellPoint
    {
        public CellPoint(Vector2Int size) => 
            Size = size;

        public Vector2Int Size { get; private set; }
    }
}