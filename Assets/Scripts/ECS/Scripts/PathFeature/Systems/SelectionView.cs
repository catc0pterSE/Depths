using UnityEngine;

namespace ECS.Scripts.PathFeature.Systems
{
    public sealed class SelectionView : MonoBehaviour
    {
        [SerializeField] private RectTransform _box;
        
        public void SetScreen(Vector2 startPos, Vector2 endPos)
        {
            float width = endPos.x - startPos.x;
            float height = endPos.y - startPos.y;
            _box.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
            _box.anchoredPosition = startPos + new Vector2( width / 2, height / 2);
        }
    }
}