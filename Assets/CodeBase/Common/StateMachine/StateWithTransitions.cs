using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace CodeBase.Common.StateMachine
{
    public abstract class StateWithTransitions
    {
        private readonly IEnumerable<ITransition> _transitions;

        public StateWithTransitions(IEnumerable<ITransition> transitions) =>
            _transitions = transitions;
        
        public virtual async UniTask Enter() =>
           await EnableTransitions();

        public virtual async UniTask Exit() =>
           await DisableTransitions();

        private async UniTask EnableTransitions() =>
            await UniTask.WhenAll(_transitions.Select(transition => transition.Enable()));

        private async UniTask DisableTransitions() =>
            await UniTask.WhenAll(_transitions.Select(transition => transition.Disable()));
        
    }
}