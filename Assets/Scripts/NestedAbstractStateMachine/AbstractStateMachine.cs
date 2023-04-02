using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NestedAbstractStateMachineGenericLess
{
    public interface IAbstractState
    {
        public void OnEnter();

        public void OnUpdate();

        public void OnFixedUpdate();

        public void OnExit();
    }



    public abstract class AbstractStateMachine : IAbstractState
    {
        private AbstractStateMachine _parentStateMachine;
        private IAbstractState[] _states;
        private int _defaultState;
        private int _currentState = -1;

        public void Init<T>(AbstractStateMachine parentStateMachine, T defaultStateIndex, params IAbstractState[] states) where T : struct, System.IConvertible
        {
            _parentStateMachine = parentStateMachine;
            _states = states;
            _defaultState = (int)(object)defaultStateIndex;
        }
        public T GetCurrentStateIndex<T>() where T : struct, System.IConvertible
        {
            return (T)(object)_currentState;
        }
        public T[] GetCurrentHierarchicalStates<T>() where T : IAbstractState
        {
            List<T> indexes = new List<T>();
            IAbstractState state = _states[_currentState];
            indexes.Add((T)state);
            while (state is AbstractStateMachine machine)
            {
                state = machine.GetCurrentState<IAbstractState>();
                indexes.Add((T)state);
            }
            return indexes.ToArray();
        }
        public T GetCurrentState<T>() where T : IAbstractState
        {
            return (T)_states[_currentState];
        }
        public void OnEnter()
        {
            TransitionToState(_defaultState);
        }

        public void OnUpdate()
        {
            if (OnAnyStateUpdate(_states[_currentState]))
            {
                _states[_currentState].OnUpdate();
            }
        }
        public void OnFixedUpdate()
        {
            if (OnAnyStateFixedUpdate(_states[_currentState]))
            {
                _states[_currentState].OnFixedUpdate();
            }
        }
        public void OnExit()
        {

        }
        public abstract void OnExitFromSubStateMachine(AbstractStateMachine subStateMachine);
        public void TransitionToState<T>(T newStateEnum) where T : struct, System.IConvertible
        {
            if (_currentState > -1 && OnAnyStateExit(_states[_currentState]))
            {
                _states[_currentState].OnExit();
            }
            _currentState = (int)(object)newStateEnum;
            if (_currentState == -1)
            {
                if (_parentStateMachine == null)
                {
                    TransitionToState(_defaultState);
                }
                else
                {
                    _parentStateMachine.OnExitFromSubStateMachine(this);
                }
            }
            else if (OnAnyStateEnter(_states[_currentState]))
            {
                _states[_currentState].OnEnter();
            }
        }

        protected virtual bool OnAnyStateEnter(IAbstractState state)
        {
            return true;
        }
        protected virtual bool OnAnyStateUpdate(IAbstractState state)
        {
            return true;
        }
        protected virtual bool OnAnyStateFixedUpdate(IAbstractState state)
        {
            return true;
        }
        protected virtual bool OnAnyStateExit(IAbstractState state)
        {
            return true;
        }
    }
}
