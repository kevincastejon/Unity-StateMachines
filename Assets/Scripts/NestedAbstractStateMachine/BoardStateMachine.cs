using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NestedAbstractStateMachineGenericLess
{
    public class _BoardStateMachine : AbstractStateMachine
    {
        public enum BoardState
        {
            WAITING,
            CHECKING,
        }
        public _BoardStateMachine(AbstractStateMachine parentStateMachine)
        {
            Init(parentStateMachine, BoardState.WAITING, new WaitingState(this), new CheckingState(this));
        }

        public override void OnExitFromSubStateMachine(AbstractStateMachine subStateMachine)
        {
            
        }
        public class WaitingState : IAbstractState
        {
            private _BoardStateMachine StateMachine { get; set; }
            private void TransitionToState(BoardState state) { StateMachine.TransitionToState(state); }
            public WaitingState(_BoardStateMachine stateMachine)
            {
                StateMachine = stateMachine;
            }

            public void OnEnter()
            {

            }

            public void OnUpdate()
            {
                if (Input.anyKeyDown)
                {
                    TransitionToState(BoardState.CHECKING);
                    return;
                }
            }

            public void OnFixedUpdate()
            {

            }

            public void OnExit()
            {

            }
        }


        public class CheckingState : IAbstractState
        {
            private _BoardStateMachine StateMachine { get; set; }
            private void TransitionToState(BoardState state) { StateMachine.TransitionToState(state); }
            public CheckingState(_BoardStateMachine stateMachine)
            {
                StateMachine = stateMachine;
            }
            public void OnEnter()
            {

            }

            public void OnUpdate()
            {
                if (Input.anyKeyDown)
                {
                    StateMachine.TransitionToState(-1);
                    return;
                }
            }

            public void OnFixedUpdate()
            {

            }

            public void OnExit()
            {

            }
        }
       

    }
}