using System.Collections.Generic;
using UnityEngine;

namespace ECS.Scripts.PathFeature.Components
{
    public struct UpdatePath
    {
    }
    public struct EntityPath
    {
        public int index;
        public List<Vector3> value;
    }
}