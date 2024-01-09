using UnityEngine;

public class CameraRay : MonoBehaviour
{
    private const int MaxDistance = 100;

    [SerializeField] private LayerMask _layerMask;

    private Camera _camera;

    private void Awake() =>
        _camera = Camera.main;

    public PathNode GetNode()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, MaxDistance, _layerMask) == false)
            return null;

        if (hit.transform.gameObject.TryGetComponent(out PathNode node) == false)
            return null;

        return node;
    }
}