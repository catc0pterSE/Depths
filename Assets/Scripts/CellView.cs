using UnityEngine;

public class CellView : MonoBehaviour
{
    private Renderer _renderer;
    private Color _defaultColor;
    
    public Vector2Int MapPosition { get; private set; }
    public Vector3 Position => transform.position;

    private void Awake() =>
        _renderer = GetComponent<Renderer>();

    public void Initialize(int positionX, int positionY) => 
        MapPosition = new Vector2Int(positionX, positionY);

    public void SetDefaultColor(Color color)
    {
        _defaultColor = color;
        SetColor(_defaultColor);
    }

    public void SetColor(Color color) =>
        _renderer.material.color = color;
}