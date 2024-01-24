using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public interface IPathFindingService
    {
        void SetNotWalkingNode();
        List<Vector3> FindPath(Vector3 startPosition, Vector3 finishPosition);
    }
}