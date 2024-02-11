using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.StateMachine
{
    public interface IGameStateMachine
    {
        public UniTask EnterBootStrapState();

        public UniTask EnterMaiMenuState();
    }
}