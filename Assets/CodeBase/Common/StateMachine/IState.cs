using Cysharp.Threading.Tasks;

namespace CodeBase.Common.StateMachine
{
    public interface IState
    {
        UniTask Exit();
    }
}