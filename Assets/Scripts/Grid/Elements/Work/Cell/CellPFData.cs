using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid.Elements.Work.Cell
{
    [CreateAssetMenu(menuName = "Element Game", fileName = "CellPFData", order = 0)]
    public class CellPFData : ScriptableObject
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public TileBase TileBaseCell { get; private set; }
        [field: SerializeField] public int CostCell { get; private set; }
        [field: SerializeField] public bool IsBuild { get; private set; }
    }
}