using UnityEngine;

namespace Grid.Elements
{
    public class CellView : MonoBehaviour
    {
        private Renderer _renderer;
        public Color DefaultColor { get; private set; } = Color.white;
        public CellPFModel CellPfModel { get; private set; }

        public Vector2Int MapPosition { get; private set; }
        public Vector3 Position => transform.position;

        private void Awake() =>
            _renderer = GetComponent<Renderer>();

        public void Initialize(int positionX, int positionY, CellPFModel cellPfModel)
        {
            MapPosition = new Vector2Int(positionX, positionY);
            CellPfModel = cellPfModel;
        }

        public void SetDefaultColor(Color color)
        {
            DefaultColor = color;
            SetColor(DefaultColor);
        }

        public void SetColor(Color color) =>
            _renderer.material.color = color;
    }
}