using UnityEngine;

public class GridPN : IGrid
{
    private readonly PoolPathNods _poolPathNods;
    private readonly PathNode[,] _map;

    public GridPN(Vector2Int size, PoolPathNods poolPathNods)
    {
        _map = new PathNode[size.x, size.y];
        _poolPathNods = poolPathNods;
    }

    public void Create()
    {
        for (var x = 0; x < _map.GetLength(0); x++)
        {
            for (var y = 0; y < _map.GetLength(0); y++)
            {
                var pathNode = _poolPathNods.GetFreeNode();
                pathNode.transform.position = new Vector3(x, 0, y);

                _map[x, y] = pathNode;
                pathNode.Initialize(x, y);

                SetColorMap(pathNode, Color.grey);
            }
        }
    }

    private static void SetColorMap(PathNode pathNode, Color color)
    {
        int X = pathNode.ArrayPosition.x;
        int Y = pathNode.ArrayPosition.y;

        if ((X % 2 != 0 && Y % 2 == 0) || (X % 2 == 0 && Y % 2 != 0))
            pathNode.DefaultColor(color);
    }

    public PathNode GetElement(int arrayPositionX, int arrayPositionY)
    {
        if (arrayPositionX < 0 || arrayPositionY < 0)
            return null;

        if (arrayPositionX > _map.GetLength(0) - 1 || arrayPositionY > _map.GetLength(1) - 1)
            return null;

        return _map[arrayPositionX, arrayPositionY];
    }

    public PathNode GetElement(Vector3 position)
    {
        PathNode pathNode = null;

        foreach (var element in _map)
            if (element.Position == position)
                pathNode = element;

        return pathNode;
    }
}