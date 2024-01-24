using System;
using System.Collections.Generic;
using Grid.Elements;
using Grid.Elements.Work.Cell;
using UnityEngine.Pool;

namespace ECS.Scripts.Data
{
    [Serializable]
    public sealed class RuntimeData
    {
        public float deltaTime;

        public ListPool<List<CellPFModel>> pathPool = new ListPool<List<CellPFModel>>();
    }
}