using UnityEngine;

namespace Grid.Quad
{
    public class Rectangle
    {
        public Vector2Int _startPosition { get; private set; }
        public int _height { get; private set; }
        public int _width { get; private set; }

        public Rectangle(Vector2Int startPosition, int width, int height)
        {
            _startPosition = startPosition;
            _width = width;
            _height = height;
        }

        public bool Contains(Point point) =>
            point.Size.x >= _startPosition.x - _width && point.Size.x <= _startPosition.x + _width &&
            point.Size.y >= _startPosition.y - _height && point.Size.y <= _startPosition.y + _height;
    }
}