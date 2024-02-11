using System;
using System.Collections.Generic;
using CodeBase.Common.StateMachine;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.States;

namespace CodeBase.Common.Factories.GameStateMachineFactories
{
    public class GameStatesFactory : IStatesFactory
    {
        private readonly IGameStateMachine _gameStateMachine;
        private Dictionary<Type, Func<IState>> _typedCreationStrategies;


        public GameStatesFactory(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            InitializeStates();
        }

        private void InitializeStates()
        {
            _typedCreationStrategies = new Dictionary<Type, Func<IState>>()
            {
                [typeof(BootstrapState)] = () => new BootstrapState(_gameStateMachine)
            };
        }

        public IState Create<T>() where T : IState
        {
            if (!_typedCreationStrategies.TryGetValue(typeof(T), out Func<IState> strategy))
                throw new ArgumentException($"No valid state creation func assigned to type {typeof(T)}");

            return strategy.Invoke();
        }
    }
}