using Cysharp.Threading.Tasks;

namespace CodeBase.Common.StateMachine
{
    public interface IParameterState<TPayLoad> : IState
    {
        UniTask Enter(TPayLoad payload);
    }

    public interface IParameterState<TPayLoad1, TPayload2> : IState
    {
        UniTask Enter(TPayLoad1 payload1, TPayload2 payload2);
    }
}