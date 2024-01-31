using UnityEngine;

namespace Grid.Quad
{
    public class Rectangle
    {
        public Vector2Int _startPosition { get; }
        public int _height { get; }
        public int _width { get; }

        public Rectangle(Vector2Int startPosition, int width, int height)
        {
            _startPosition = startPosition;
            _width = width;
            _height = height;
        }

        public bool Contains(CellPoint cellPoint) =>
            cellPoint.Size.x >= _startPosition.x - _width && cellPoint.Size.x <= _startPosition.x + _width &&
            cellPoint.Size.y >= _startPosition.y - _height && cellPoint.Size.y <= _startPosition.y + _height;

        public bool Intersects(Rectangle rectangle) =>
            !(rectangle._startPosition.x - rectangle._width > _startPosition.x + _width  ||
              rectangle._startPosition.x + rectangle._width < _startPosition.x - _width ||
              rectangle._startPosition.y - rectangle._height > _startPosition.y + _height ||
              rectangle._startPosition.y + rectangle._height < _startPosition.y - _height);
    }
}