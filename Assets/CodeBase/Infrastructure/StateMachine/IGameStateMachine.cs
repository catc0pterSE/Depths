using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.StateMachine
{
    public interface IGameStateMachine
    {
        UniTask EnterBootStrapState();
    }
}