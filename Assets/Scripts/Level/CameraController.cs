using UnityEngine;

namespace Level
{
    public class CameraController : MonoBehaviour 
    {
        private Camera _camera;

        private void Awake() =>
            _camera = Camera.main;

        public Vector3 GetWorldPosition() => 
            _camera.ScreenToWorldPoint(Input.mousePosition);
    }
}