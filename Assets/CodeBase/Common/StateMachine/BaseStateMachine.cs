using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace CodeBase.Common.StateMachine
{
    public abstract class BaseStateMachine
    {
        private IState _currentState;
        private Dictionary<Type, IState> _states;

        public void RegisterStates (IEnumerable<IState> states) =>
            _states = states.ToDictionary(state => state.GetType(), state => state);

        protected async UniTask Enter<TState>() where TState : class, IParameterlessState
        {
            TState newState = await ChangeState<TState>();
            await newState.Enter();
        }

        protected async UniTask Enter<TState, TPayload>(TPayload payload)
            where TState : class, IParameterState<TPayload>
        {
            TState newState = await ChangeState<TState>();
            await newState.Enter(payload);
        }

        protected async UniTask Enter<TState, TPayload1, TPayload2>(TPayload1 payload1, TPayload2 payload2)
            where TState : class, IParameterState<TPayload1, TPayload2>
        {
            TState newState = await ChangeState<TState>();
            await newState.Enter(payload1, payload2);
        }

        private async UniTask<TState> ChangeState<TState>() where TState : class, IState
        {
            TState nextState = GetState<TState>();
            await ExitCurrentState();
            SetCurrentState(nextState);
            return nextState;
        }

        private TState GetState<TState>() where TState : class, IState =>
            _states[typeof(TState)] as TState;

        private async UniTask ExitCurrentState()
        {
            if (_currentState != null)
                await _currentState.Exit();
        }

        private void SetCurrentState(IState state) =>
            _currentState = state;
    }
}