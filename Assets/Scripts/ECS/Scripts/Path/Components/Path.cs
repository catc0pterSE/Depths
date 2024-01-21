using System.Collections.Generic;
using Grid.Elements;
using UnityEngine;

namespace ECS.Scripts.Path.Components
{
    public struct UpdatePath
    {
    }
    public struct Path
    {
        public int index;
        public List<Vector3> value;
    }
}