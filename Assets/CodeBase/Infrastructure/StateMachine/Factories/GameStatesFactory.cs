using System;
using System.Collections.Generic;
using CodeBase.Common.StateMachine;
using CodeBase.Infrastructure.StateMachine.States;

namespace CodeBase.Infrastructure.StateMachine.Factories
{
    public class GameStatesFactory : IGameStatesFactory
    {
        private Dictionary<Type, Func<IGameStateMachine, IState>> _typedCreationStrategies =
            new Dictionary<Type, Func<IGameStateMachine, IState>>()
            {
                [typeof(BootstrapState)] = (gameStateMachine) => new BootstrapState(gameStateMachine)
            };

        public IState Create<T>(IGameStateMachine context) where T : IState
        {
            if (!_typedCreationStrategies.TryGetValue(typeof(T), out Func<IGameStateMachine, IState> strategy))
                throw new ArgumentException($"No state creation func assigned to type {typeof(T)}");

            return strategy.Invoke(context);
        }
    }
}