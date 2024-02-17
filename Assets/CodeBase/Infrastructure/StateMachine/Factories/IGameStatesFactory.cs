using CodeBase.Common.StateMachine;

namespace CodeBase.Infrastructure.StateMachine.Factories
{
    public interface IGameStatesFactory
    {
        IState Create<T>(IGameStateMachine context) where T : IState;
    }
}