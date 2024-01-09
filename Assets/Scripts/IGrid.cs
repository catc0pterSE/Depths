using UnityEngine;

public interface IGrid
{
    public void Create();
    public PathNode GetElement(int arrayPositionX, int arrayPositionY);
    public PathNode GetElement(Vector3 position);
}