using System.Collections.Generic;
using UnityEngine;

namespace ECS.Boot
{
    public struct Path
    {
        public int index;
        public List<CellView> value;
    }

    public struct TargetPath
    {
        public Vector3 value;
    }
}