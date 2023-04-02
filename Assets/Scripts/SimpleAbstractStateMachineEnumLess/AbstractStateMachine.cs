using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleAbstractStateMachineGenericLess
{
    public class AbstractState
    {
        private AbstractStateMachine _parentStateMachine;

        protected AbstractState(AbstractStateMachine parentStateMachine)
        {
            _parentStateMachine = parentStateMachine;
        }

        protected void TransitionToState<T>(T newStateEnum) where T : struct, System.IConvertible
        {
            ParentStateMachine.TransitionToState((int)(object)newStateEnum);
        }
        public AbstractStateMachine ParentStateMachine { get => _parentStateMachine; }

        public virtual void OnEnter() { }

        public virtual void OnUpdate() { }

        public virtual void OnFixedUpdate() { }

        public virtual void OnExit() { }
    }



    public class AbstractStateMachine : MonoBehaviour
    {
        private AbstractState[] _states;
        private int _currentState;
        public AbstractState CurrentState { get => _states[_currentState]; }
        protected T GetCurrentStateIndex<T>() where T : struct, System.IConvertible
        {
            return (T)(object)_currentState;
        }
        protected void SetStates(params AbstractState[] states)
        {
            _states = states;
        }
        protected void SetDefaultState<T>(T defaultStateIndex) where T : struct, System.IConvertible
        {
            _currentState = (int)(object)defaultStateIndex;
        }
        private void Start()
        {
            if (OnAnyStateEnter(CurrentState))
            {
                CurrentState.OnEnter();
            }
        }

        private void Update()
        {
            if (OnAnyStateUpdate(CurrentState))
            {
                CurrentState.OnUpdate();
            }
        }
        private void FixedUpdate()
        {
            if (OnAnyStateFixedUpdate(CurrentState))
            {
                CurrentState.OnFixedUpdate();
            }
        }
        public void TransitionToState(int newStateIndex)
        {
            if (OnAnyStateExit(CurrentState))
            {
                CurrentState.OnExit();
            }
            _currentState = newStateIndex;
            if (OnAnyStateEnter(CurrentState))
            {
                CurrentState.OnEnter();
            }
        }

        protected virtual bool OnAnyStateEnter(AbstractState state)
        {
            return true;
        }
        protected virtual bool OnAnyStateUpdate(AbstractState state)
        {
            return true;
        }
        protected virtual bool OnAnyStateFixedUpdate(AbstractState state)
        {
            return true;
        }
        protected virtual bool OnAnyStateExit(AbstractState state)
        {
            return true;
        }
    }
}
