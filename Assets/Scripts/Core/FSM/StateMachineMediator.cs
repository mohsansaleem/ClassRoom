﻿using System;
using System.Collections.Generic;
using PG.Core.Installer;
using UniRx;
using UnityEngine;
using Zenject;

namespace PG.Core.FSM
{
    public partial class StateMachineMediator : IInitializable, ITickable, IDisposable
    {
        protected StateBehaviour CurrentStateBehaviour;
        protected Dictionary<int, StateBehaviour> StateBehaviours = new Dictionary<int, StateBehaviour>();
        
        protected CompositeDisposable Disposables;

        [Inject] protected CoreSceneInstaller SceneInstaller;
        [Inject] protected readonly SignalBus SignalBus;
        [Inject] protected readonly Signal.Factory SignalFactory;

        public virtual void Initialize()
		{
		    Disposables = new CompositeDisposable();
            SignalBus.Subscribe<RequestStateChangeSignal>(GoToState);
        }

        public virtual void GoToState(RequestStateChangeSignal signal)
        {
            GoToState(signal.stateType);
        }

        protected void GoToState(int stateType)
        {
            if (!StateBehaviours.ContainsKey(stateType))
            {
                Debug.LogWarning($"State Missing in Mediator. Id: {stateType}");
            }
            else if(CurrentStateBehaviour == null || StateBehaviours[stateType] != CurrentStateBehaviour)
            {
                GoToStateInternal(stateType);
            }
        }
        
        private void GoToStateInternal(int stateType)
        {
            if (StateBehaviours.ContainsKey(stateType))
            {
                if (CurrentStateBehaviour != null)
                {
                    CurrentStateBehaviour.OnStateExit();
                }
                CurrentStateBehaviour = StateBehaviours[stateType];
                
                CurrentStateBehaviour.OnStateEnter();
            }
            else
            {
                Debug.LogError($"State Id[{stateType}] doesn't Exist in the Dictionary.");
            }
        }

        public virtual void Tick()
        {
            CurrentStateBehaviour?.Tick();
        }

        public virtual void Dispose()
        {
            SignalBus.Unsubscribe<RequestStateChangeSignal>(GoToState);

            if (Application.isPlaying)
                CurrentStateBehaviour?.OnStateExit();

            Disposables.Dispose();

            StateBehaviours.Clear();
        }
    }
}
