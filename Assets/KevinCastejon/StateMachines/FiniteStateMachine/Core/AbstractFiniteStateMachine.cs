using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinCastejon.FiniteStateMachine
{
    public abstract class AbstractState
    {
        /// <summary>
        /// Constant value to use on the TransitionToState sub state machine method parameter to make it exit. You can also directly pass the -1 value.
        /// </summary>
        public const int EXIT = -1;
        protected AbstractFiniteStateMachine _parentStateMachine;
        private string _name = null;
        private int _index = -1;

        /// <summary>
        /// The state (or subStateMachine) enum name
        /// </summary>
        public string Name { get => _name; }
        /// <summary>
        /// The state (or subStateMachine) enum index
        /// </summary>
        public int Index { get => _index; }
        /// <summary>
        /// Returns the state (or subStateMachine) enum value
        /// </summary>
        /// <typeparam name="T">The state (or subStateMachine) enum type</typeparam>
        /// <returns>The state (or subStateMachine) enum value</returns>
        public T GetEnumValue<T>() where T : System.Enum
        {
            return (T)(object)_index;
        }
        /// <summary>
        /// Returns the parent stateMachine of the state (or subStateMachine)
        /// </summary>
        public T GetStateMachine<T>() where T : AbstractFiniteStateMachine { return (T)_parentStateMachine; }
        /// <summary>
        /// Creates and returns a new state (or subStateMachine)
        /// </summary>
        /// <typeparam name="T0">The state (or subStateMachine) type</typeparam>
        /// <typeparam name="T1">The state (or subStateMachine) enum type</typeparam>
        /// <param name="enumValue">The state (or subStateMachine) enum value</param>
        /// <param name="parentStateMachine">The parent state machine of the state (or subStateMachine)</param>
        /// <returns>A newly created state (or subStateMachine)</returns>
        public static T0 Create<T0, T1>(T1 enumValue, AbstractFiniteStateMachine parentStateMachine) where T0 : AbstractState, new() where T1 : System.Enum
        {
            T0 newState = new T0();
            newState._name = enumValue.ToString();
            newState._index = (int)(object)enumValue;
            newState._parentStateMachine = parentStateMachine;
            return newState;
        }
        /// <summary>
        /// Transition from the current state to another state
        /// </summary>
        /// <typeparam name="T">An enum value or index of the destination state</typeparam>
        /// <param name="newStateEnum">The destination state</param>
        public void TransitionToState<T>(T newStateEnum) where T : struct, System.IConvertible
        {
            _parentStateMachine.TransitionToState<T>(newStateEnum);
        }
        /// <summary>
        /// This method is called when this state is entered
        /// </summary>
        public virtual void OnEnter()
        {
        }
        /// <summary>
        /// This method is called at each update into this state
        /// </summary>
        public virtual void OnUpdate()
        {
        }
        /// <summary>
        /// This method is called at each fixed update into this state
        /// </summary>
        public virtual void OnFixedUpdate()
        {
        }
        /// <summary>
        /// This method is called when this state is exited
        /// </summary>
        public virtual void OnExit()
        {
        }
    }



    public class AbstractFiniteStateMachine : MonoBehaviour
    {
        private AbstractState[] _states;
        private int _defaultState;
        private int _currentState;
        private void Start()
        {
            if (OnAnyStateEnter())
            {
                _states[_currentState].OnEnter();
            }
        }
        private void Update()
        {
            if (OnAnyStateUpdate())
            {
                _states[_currentState].OnUpdate();
            }
        }
        private void FixedUpdate()
        {
            if (OnAnyStateFixedUpdate())
            {
                _states[_currentState].OnFixedUpdate();
            }
        }
        /// <summary>
        /// Initializes the state machine
        /// </summary>
        /// <typeparam name="T">The Enum type for this stateMachine</typeparam>
        /// <param name="defaultStateIndex">The state machine default state enum value</param>
        /// <param name="states">The state machine states</param>
        public void Init<T>(T defaultStateIndex, params AbstractState[] states) where T : System.Enum
        {
            _states = new AbstractState[states.Length];
            for (int i = 0; i < states.Length; i++)
            {
                _states[states[i].Index] = states[i];
            }
            _defaultState = (int)(object)defaultStateIndex;
        }
        /// <summary>
        /// Returns the current state
        /// </summary>
        /// <returns>The current state</returns>
        public AbstractState GetCurrentState()
        {
            return _states[_currentState];
        }
        /// <summary>
        /// Returns the current state enum index
        /// </summary>
        /// <returns>The current state</returns>
        public int GetCurrentStateEnumIndex()
        {
            return _currentState;
        }
        /// <summary>
        /// Returns the current state enum value
        /// </summary>
        /// <typeparam name="T">The state enum type</typeparam>
        /// <returns>The current state enum value</returns>
        public T GetCurrentStateEnumValue<T>() where T : System.Enum
        {
            return _states[_currentState].GetEnumValue<T>();
        }
        /// <summary>
        /// Returns the current state enum name
        /// </summary>
        /// <returns>The current state enum name</returns>
        public string GetCurrentStateName()
        {
            return _states[_currentState].Name;
        }
        /// <summary>
        /// Transition from the current state to another state
        /// </summary>
        /// <typeparam name="T">An enum value or index of the destination state</typeparam>
        /// <param name="newStateEnum">The destination state</param>
        public void TransitionToState<T>(T newStateEnum) where T : struct, System.IConvertible
        {
            if (_currentState > -1 && OnAnyStateExit())
            {
                _states[_currentState].OnExit();
            }
            _currentState = (int)(object)newStateEnum;
            if (_currentState == -1)
            {
                _currentState = _defaultState;
            }
            else if (OnAnyStateEnter())
            {
                _states[_currentState].OnEnter();
            }
        }
        /// <summary>
        /// This method is called before specific states OnEnter methods. 
        /// </summary>
        /// <returns>Return if the state specific method should be called</returns>
        protected virtual bool OnAnyStateEnter()
        {
            return true;
        }
        /// <summary>
        /// This method is called before specific states OnUpdate methods. 
        /// </summary>
        /// <returns>Return if the state specific method should be called</returns>
        protected virtual bool OnAnyStateUpdate()
        {
            return true;
        }
        /// <summary>
        /// This method is called before specific states OnFixedUpdate methods. 
        /// </summary>
        /// <returns>Return if the state specific method should be called</returns>
        protected virtual bool OnAnyStateFixedUpdate()
        {
            return true;
        }
        /// <summary>
        /// This method is called before specific states OnExit methods. 
        /// </summary>
        /// <returns>Return if the state specific method should be called</returns>
        protected virtual bool OnAnyStateExit()
        {
            return true;
        }
    }
}
