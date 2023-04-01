using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace NestedAbstractStateMachine
{
    public abstract class AbstractState
    {
        private UnityAction<int> _transitionToState;
        private UnityAction _exit;

        protected UnityAction<int> TransitionToState { get => _transitionToState; }
        protected UnityAction Exit { get => _exit; }

        public void SetCallback(UnityAction<int> transitionToState, UnityAction exit)
        {
            _transitionToState = transitionToState;
            _exit = exit;
        }
        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void OnUpdate() { }
    }
    public abstract class NestedAbstractStateMachine : AbstractState
    {
        private Dictionary<int, AbstractState> _states;
        private int _currentState;
        public int CurrentState { get => _currentState; private set => _currentState = value; }
        protected void SetStates(Dictionary<int, AbstractState> states)
        {
            _states = states;
            foreach (var state in _states)
            {
                state.Value.SetCallback(TransitionToState, ExitToParent);
            }
        }
        public void Update()
        {
            if (OnAnyStateUpdate(CurrentState))
            {
                _states[CurrentState].OnUpdate();
            }
        }
        protected abstract void ExitFromSubStateMachine(int subStateMachine);
        protected void ExitToParent()
        {

        }
        protected new void TransitionToState(int newState)
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
        protected virtual bool OnAnyStateEnter(int state)
        {
            return true;
        }
        protected virtual bool OnAnyStateUpdate(int state)
        {
            return true;
        }
        protected virtual bool OnAnyStateExit(int state)
        {
            return true;
        }
        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }

    }
}
