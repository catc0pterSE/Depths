using Sources.Infrastructure.Core.Services.DI;
using UnityEngine;

namespace Sources.Application.CompositionRoots
{
    public abstract class SceneCompositionRoot : MonoBehaviour
    {
        public abstract void Initialize(ServiceContainer serviceContainer);
    }
}