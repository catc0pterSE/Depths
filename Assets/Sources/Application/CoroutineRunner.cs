using Sources.Infrastructure.Api.Services;
using UnityEngine;

namespace Sources.Application
{
    internal class CoroutineRunner : MonoBehaviour, ICoroutineRunner
    {
        private void Awake() => 
            DontDestroyOnLoad(this);
    }
}