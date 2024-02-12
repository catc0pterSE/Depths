using System;
using System.Collections.Generic;
using CodeBase.Common.StateMachine;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.States;

namespace CodeBase.Common.Factories.GameStateMachineFactories
{
    public class GameStatesFactory : IStatesFactory
    {
        private Dictionary<Type, Func<IGameStateMachine, IState>> _typedCreationStrategies =
            new Dictionary<Type, Func<IGameStateMachine, IState>>()
            {
                [typeof(BootstrapState)] = (gameStateMachine) => new BootstrapState(gameStateMachine)

            };

        public IState Create<T>(BaseStateMachine context) where T : IState
        {
            if (context is not GameStateMachine gameStateMachine)
                throw new ArgumentException($"StateMachine of type {context.GetType()} is not applicable to {GetType()}");
            
            if (!_typedCreationStrategies.TryGetValue(typeof(T), out Func<IGameStateMachine, IState> strategy))
                throw new ArgumentException($"No valid state creation func assigned to type {typeof(T)}");

            return strategy.Invoke(gameStateMachine);
        }
    }
}