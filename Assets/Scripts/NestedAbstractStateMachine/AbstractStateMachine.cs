using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NestedAbstractStateMachineGenericLess
{
    public abstract class AbstractState
    {
        private string _name;

        public string Name { get => _name; set => _name = value; }

        public AbstractState(string name)
        {
            _name = name;
        }
        public abstract void OnEnter();

        public abstract void OnUpdate();

        public abstract void OnFixedUpdate();

        public abstract void OnExit();
    }



    public abstract class AbstractStateMachine : AbstractState
    {
        private AbstractStateMachine _parentStateMachine;
        private AbstractState[] _states;
        private int _defaultState;
        private int _currentState = -1;

        protected AbstractStateMachine(string name) : base(name)
        {

        }

        public void Init<T>(T defaultStateIndex, params AbstractState[] states) where T : System.Enum
        {
            Init(null, defaultStateIndex, states);
        }
        public void Init<T>(AbstractStateMachine parentStateMachine, T defaultStateIndex, params AbstractState[] states) where T : System.Enum
        {
            _parentStateMachine = parentStateMachine;
            _states = states;
            _defaultState = (int)(object)defaultStateIndex;
        }
        public T GetCurrentState<T>() where T : AbstractState
        {
            return (T)_states[_currentState];
        }
        public int GetCurrentStateEnumIndex()
        {
            return _currentState;
        }
        public T GetCurrentStateEnumValue<T>() where T : System.Enum
        {
            return (T)(object)_currentState;
        }
        public string GetCurrentStateName()
        {
            return _states[_currentState].Name;
        }
        public T[] GetCurrentHierarchicalStates<T>() where T : AbstractState
        {
            List<T> indexes = new List<T>();
            AbstractState state = _states[_currentState];
            indexes.Add((T)state);
            while (state is AbstractStateMachine machine)
            {
                state = machine.GetCurrentState<AbstractState>();
                indexes.Add((T)state);
            }
            return indexes.ToArray();
        }
        public int[] GetCurrentHierarchicalStatesIndexes()
        {
            List<int> indexes = new List<int>();
            AbstractState state = _states[_currentState];
            indexes.Add(_currentState);
            while (state is AbstractStateMachine machine)
            {
                state = machine.GetCurrentState<AbstractState>();
                indexes.Add(machine.GetCurrentStateEnumIndex());
            }
            return indexes.ToArray();
        }
        public string[] GetCurrentHierarchicalStatesNames()
        {
            List<string> names = new List<string>();
            AbstractState state = _states[_currentState];
            names.Add(state.Name);
            while (state is AbstractStateMachine machine)
            {
                state = machine.GetCurrentState<AbstractState>();
                names.Add(state.Name);
            }
            return names.ToArray();
        }
        public string GetCurrentHierarchicalStatesNamesString(string separator = " > ")
        {
            string str = "";
            AbstractState state = _states[_currentState];
            str += state.Name;
            if (state is AbstractStateMachine)
            {
                str += separator;
            }
            while (state is AbstractStateMachine machine)
            {
                state = machine.GetCurrentState<AbstractState>();
                str += state.Name;
                if (state is AbstractStateMachine)
                {
                    str += separator;
                }
            }
            return str;
        }

        public override void OnEnter()
        {
            TransitionToState(_defaultState);
        }

        public override void OnUpdate()
        {
            if (OnAnyStateUpdate(_states[_currentState]))
            {
                _states[_currentState].OnUpdate();
            }
        }
        public override void OnFixedUpdate()
        {
            if (OnAnyStateFixedUpdate(_states[_currentState]))
            {
                _states[_currentState].OnFixedUpdate();
            }
        }
        public override void OnExit()
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
