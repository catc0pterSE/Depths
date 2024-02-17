using Cysharp.Threading.Tasks;

namespace CodeBase.Common.StateMachine
{
    public interface IParameterState<TPayload> : IState
    {
        UniTask Enter(TPayload payload);
    }

    public interface IParameterState<TPayload1, TPayload2> : IState
    {
        UniTask Enter(TPayload1 payload1, TPayload2 payload2);
    }
}