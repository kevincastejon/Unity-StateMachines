using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NestedAbstractStateMachineGenericLess
{
    public abstract class AbstractState
    {
        public const int EXIT = -1;
        protected AbstractStateMachine _parentStateMachine;
        protected string _name;
        protected int _index;

        public string Name { get => _name; }
        public int Index { get => _index; }
        public T GetEnumValue<T>() where T : System.Enum
        {
            return (T)(object)_index;
        }
        public T GetStateMachine<T>() where T : AbstractStateMachine { return (T)_parentStateMachine; }

        public static T0 Create<T0, T1>(T1 enumValue, AbstractStateMachine parentStateMachine) where T0 : AbstractState, new() where T1 : System.Enum
        {
            T0 newState = new T0();
            newState._name = enumValue.ToString();
            newState._index = (int)(object)enumValue;
            newState._parentStateMachine = parentStateMachine;
            return newState;
        }
        public static T0 Create<T0>(string name) where T0 : AbstractState, new()
        {
            T0 newState = new T0();
            newState._name = name;
            newState._index = -1;
            newState._parentStateMachine = null;
            return newState;
        }
        public virtual void OnEnter()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnFixedUpdate()
        {
        }

        public virtual void OnExit()
        {
        }
    }



    public abstract class AbstractStateMachine : AbstractState
    {
        private AbstractState[] _states;
        private int _defaultState;
        private int _currentState = -1;

        public void Init<T>(T defaultStateIndex, params AbstractState[] states) where T : System.Enum
        {
            _states = new AbstractState[states.Length];
            for (int i = 0; i < states.Length; i++)
            {
                _states[states[i].Index] = states[i];
            }
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
            return _states[_currentState].GetEnumValue<T>();
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

        public sealed override void OnEnter()
        {
            OnStateMachineEntry();
            TransitionToState(_defaultState);
        }

        public sealed override void OnUpdate()
        {
            if (OnAnyStateUpdate(_states[_currentState]))
            {
                _states[_currentState].OnUpdate();
            }
        }
        public sealed override void OnFixedUpdate()
        {
            if (OnAnyStateFixedUpdate(_states[_currentState]))
            {
                _states[_currentState].OnFixedUpdate();
            }
        }
        public sealed override void OnExit()
        {
            OnStateMachineExit();
        }
        public virtual void OnExitFromSubStateMachine(AbstractStateMachine subStateMachine)
        {
            TransitionToState(_defaultState);
        }
        public virtual void OnStateMachineEntry()
        {
        }
        public virtual void OnStateMachineExit()
        {
        }
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
