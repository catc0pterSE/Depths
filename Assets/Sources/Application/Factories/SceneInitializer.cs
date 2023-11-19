using System;
using System.Linq;
using Sources.Application.CompositionRoots;
using Sources.Infrastructure.Core.Services.DI;
using Object = UnityEngine.Object;

namespace Sources.Application
{
    public class SceneInitializer
    {
        public void Initialize(ServiceContainer serviceContainer)
        {
            SceneCompositionRoot[] compositionRoots = Object.FindObjectsOfType<SceneCompositionRoot>();

            if (compositionRoots.Length > 1)
                throw new Exception($"Scene has multiple composition roots!" +
                                    " Must use only one composition root" +
                                    $" roots:{string.Join(",", compositionRoots.Select(root => root.name))}");

            compositionRoots[0].Initialize(serviceContainer);
        }
    }
}