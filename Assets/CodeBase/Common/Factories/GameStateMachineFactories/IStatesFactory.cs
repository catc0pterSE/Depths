﻿using System;
using System.Collections.Generic;
using CodeBase.Common.StateMachine;

namespace CodeBase.Common.Factories.GameStateMachineFactories
{
    public interface IStatesFactory
    {
        IState Create<T>() where T : IState;
    }
}