using System.Collections.Generic;


// 1024 cell -> Array -> Cell -> List 8 element -> 1024 500 / 4mb  
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

    public int CalculateTotalCost() =>
        TotalCost = TransitionCost + _wayToFinalCell;
}