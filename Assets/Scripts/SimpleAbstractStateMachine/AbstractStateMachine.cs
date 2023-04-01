using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleAbstractStateMachine
{
    public abstract class AbstractState<T0> where T0 : struct, IConvertible
    {
        private UnityAction<T0> _transitionToState;
        private UnityAction _exit;
        private AbstractStateMachine<T0> _parentStateMachine;

        protected AbstractState(AbstractStateMachine<T0> parentStateMachine)
        {
            _parentStateMachine = parentStateMachine;
        }

        protected UnityAction<T0> TransitionToState { get => _transitionToState; }
        protected UnityAction Exit { get => _exit; }
        public AbstractStateMachine<T0> ParentStateMachine { get => _parentStateMachine; }

        public void SetCallback(UnityAction<T0> transitionToState, UnityAction exit)
        {
            _transitionToState = transitionToState;
            _exit = exit;
        }

        public virtual void OnEnter() { }

        public virtual void OnUpdate() { }

        public virtual void OnFixedUpdate() { }

        public virtual void OnExit() { }
    }



    public class AbstractStateMachine<T> : MonoBehaviour where T : struct, IConvertible
    {
        private Dictionary<T, AbstractState<T>> _states;
        private T _currentState;
        public T CurrentState { get => _currentState; private set => _currentState = value; }
        protected void SetStates(Dictionary<T, AbstractState<T>> states)
        {
            _states = states;
            foreach (var state in _states)
            {
                state.Value.SetCallback(TransitionToState, ExitToParent);
            }
        }
        private void Update()
        {
            if (OnAnyStateUpdate(CurrentState))
            {
                _states[CurrentState].OnUpdate();
            }
        }
        private void FixedUpdate()
        {
            if (OnAnyStateFixedUpdate(CurrentState))
            {
                _states[CurrentState].OnFixedUpdate();
            }
        }
        protected void ExitToParent()
        {

        }
        protected void TransitionToState(T newState)
        {
            if (OnAnyStateExit(CurrentState))
            {
                _states[CurrentState].OnExit();
            }

            CurrentState = newState;
            if (OnAnyStateEnter(CurrentState))
            {
                _states[CurrentState].OnEnter();
            }
        }

        protected virtual bool OnAnyStateEnter(T state)
        {
            return true;
        }
        protected virtual bool OnAnyStateUpdate(T state)
        {
            return true;
        }
        protected virtual bool OnAnyStateFixedUpdate(T state)
        {
            return true;
        }
        protected virtual bool OnAnyStateExit(T state)
        {
            return true;
        }
    }
}
