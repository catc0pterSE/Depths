using ECS.Scripts.Boot;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBuild : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject wallPrefab;
    
    private MainAspect _mainAspect;

    public void Construct(MainAspect mainAspect) => 
        _mainAspect = mainAspect;

    private void OnEnable() =>
        _button.onClick.AddListener(CreateBuild);

    private void OnDisable() =>
        _button.onClick.RemoveListener(CreateBuild);

    private void CreateBuild()
    {
        var wall = Instantiate(wallPrefab);
        
        var entity = _mainAspect.World().NewEntity();
        
        _mainAspect.Build.Add(entity).transform = wall.transform;
        _mainAspect.BuildTeg.Add(entity);
    }
}