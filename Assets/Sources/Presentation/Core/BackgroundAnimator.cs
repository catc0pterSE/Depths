using UnityEngine;

namespace Sources.Presentation.Core
{
    public class BackgroundAnimator : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private float _verticalSpeed;
        [SerializeField] private float _offsetResetThreshold;
        [SerializeField] private float _offsetResetVerticalValue;

        private float _currentOffset;

        private void Update()
        {
            _currentOffset += _verticalSpeed * Time.deltaTime;

            if (_currentOffset * _currentOffset > _offsetResetThreshold * _offsetResetThreshold)
                _currentOffset = _offsetResetVerticalValue;

            _meshRenderer.sharedMaterial.mainTextureOffset = new Vector2(0, _currentOffset);
        }
    }
}