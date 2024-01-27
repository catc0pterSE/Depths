using System.Collections.Generic;
using UnityEngine;

namespace Grid.Quad
{
    public class Quadtree
    {
        private const int Divider = 2;

        private readonly Rectangle _boundary;
        private readonly List<Point> _points;
        private List<Quadtree> _rectangles;
        private readonly int _capacity;
        private bool _divided = false;

        public Quadtree(Rectangle boundary, int capacity = 4)
        {
            _boundary = boundary;
            _capacity = capacity;
            _points = new List<Point>();
        }

        public bool Insert(Point point)
        {
            if (_boundary.Contains(point) == false)
                return false;

            if (_points.Count < _capacity)
            {
                _points.Add(point);
                return true;
            }

            if (_divided == false)
                Subdivide();

            foreach (var tree in _rectangles)
            {
                tree.Insert(point);
                return true;
            }

            return true;
        }

        private void Subdivide()
        {
            var width = _boundary._width / Divider;
            var height = _boundary._height / Divider;
            var positionX = _boundary._startPosition.x + width;
            var positionY = _boundary._startPosition.y + height;

            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    if (x == 0 || y == 0)
                        continue;

                    var directionRectangle =
                        new Rectangle(new Vector2Int(positionX * x, positionY * y), width, height);

                    _rectangles.Add(new Quadtree(directionRectangle));
                }
            }

            _divided = true;
        }
    }
}