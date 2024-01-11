using System.Collections.Generic;

public class CellData
{
    public CellData ComeFromCell;
    public bool isWalkable = true;
    
    private int _wayToFinalCell;
    
    public List<CellData> Neighbours { get; private set; }
    public int TransitionCost { get; private set; } = int.MaxValue;
    public int TotalCost { get; private set; }

    public void SetNeighbours(List<CellData> neighbours) => 
        Neighbours = neighbours;
    
    public void SetImpassable() =>
        isWalkable = false;

    public void SetTransitionCost(int cost) =>
        TransitionCost = cost;

    public void SetWayToFinalCell(int countCells) =>
        _wayToFinalCell = countCells;

    public void CalculateTotalCost() =>
        TotalCost = TransitionCost + _wayToFinalCell;
}