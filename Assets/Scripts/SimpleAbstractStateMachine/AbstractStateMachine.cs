using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleAbstractStateMachine
{
    public abstract class AbstractState<T> where T : struct, IConvertible
    {
        private UnityAction<T> _transitionToState;
        private UnityAction _exit;

        protected UnityAction<T> TransitionToState { get => _transitionToState; }
        protected UnityAction Exit { get => _exit; }

        public void SetCallback(UnityAction<T> transitionToState, UnityAction exit)
        {
            _transitionToState = transitionToState;
            _exit = exit;
        }
        public abstract void OnEnter();

        public abstract void OnExit();

        public abstract void OnUpdate();
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
                _states[CurrentState].OnEnter();
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
            if (OnAnyStateEnter(newState))
            {
                _states[newState].OnEnter();
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
        protected virtual bool OnAnyStateExit(T state)
        {
            return true;
        }
    }
}
