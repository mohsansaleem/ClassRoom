using PG.Core.Installer;
using UniRx;
using UnityEngine;
using Zenject;

namespace PG.Core.FSM
{
    public partial class StateMachineMediator
    {
        public class StateBehaviour
        {
            protected CompositeDisposable Disposables;
            protected SignalBus SignalBus;
            protected Signal.Factory SignalFactory;

            protected StateBehaviour(StateMachineMediator mediator)
            {
                SignalBus = mediator.SignalBus;
                SignalFactory = mediator.SignalFactory;
            }

            public virtual void OnStateEnter()
            {
                Debug.Log(string.Format("{0} , OnStateEnter()", this));

                Disposables = new CompositeDisposable();
            }

            public virtual void OnStateExit()
            {
                Debug.Log(string.Format("{0} , OnStateExit()", this));

                Disposables.Dispose();
            }

            public virtual bool IsValidOpenState()
            {
                return false;
            }

            public virtual void Tick()
            {

            }
        }
    }
}
