using UnityEngine;

public class PathNode : MonoBehaviour
{
    public PathNode ComeFromNode;
    public Color DefaultColorNode { get; private set; }
    public bool isWalkable = true;

    private int _wayToFinalCell;
    private Renderer _renderer;

    public Vector2Int ArrayPosition { get; private set; }
    public int TransitionCost { get; private set; } = int.MaxValue;
    public int TotalCost { get; private set; }
    public Vector3 Position => transform.position;

    private void Awake() =>
        _renderer = GetComponent<Renderer>();

    public void Initialize(int positionX, int positionY) =>
        ArrayPosition = new Vector2Int(positionX, positionY);

    public void SetTransitionCost(int cost) =>
        TransitionCost = cost;

    public void SetWayToFinalCell(int cells) =>
        _wayToFinalCell = cells;

    public void CalculateTotalCost() =>
        TotalCost = TransitionCost + _wayToFinalCell;

    public void SetImpassable() =>
        isWalkable = false;

    public void DefaultColor(Color color)
    {
        DefaultColorNode = color;
        SetColor(DefaultColorNode);
    }

    public void SetColor(Color color) =>
        _renderer.material.color = color;
}