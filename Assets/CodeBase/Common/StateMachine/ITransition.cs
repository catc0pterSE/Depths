using Cysharp.Threading.Tasks;

namespace CodeBase.Common.StateMachine
{
    public interface ITransition
    {
        UniTask Enable();
        UniTask Disable();
    }
}