using Cysharp.Threading.Tasks;

namespace CodeBase.Common.StateMachine
{
    public interface IParameterlessState: IState
    {
        UniTask Enter();
    }
}