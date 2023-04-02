using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NestedAbstractStateMachineGenericLess
{
    public class BoardStateMachine : AbstractStateMachine
    {
        public enum BoardState
        {
            WAITING,
            CHECKING,
        }
        public BoardStateMachine(string name, AbstractStateMachine parentStateMachine) : base(name)
        {
            Init(parentStateMachine, BoardState.WAITING, new WaitingState(BoardState.WAITING.ToString(), this), new CheckingState(BoardState.CHECKING.ToString(), this));
        }

        public override void OnExitFromSubStateMachine(AbstractStateMachine subStateMachine)
        {
            
        }
        public class WaitingState : AbstractState
        {
            private BoardStateMachine StateMachine { get; set; }
            private void TransitionToState(BoardState state) { StateMachine.TransitionToState(state); }
            public WaitingState(string name, BoardStateMachine stateMachine):base(name)
            {
                StateMachine = stateMachine;
            }

            public override void OnEnter()
            {

            }

            public override void OnUpdate()
            {
                if (Input.anyKeyDown)
                {
                    TransitionToState(BoardState.CHECKING);
                    return;
                }
            }

            public override void OnFixedUpdate()
            {

            }

            public override void OnExit()
            {

            }
        }


        public class CheckingState : AbstractState
        {
            private BoardStateMachine StateMachine { get; set; }
            private void TransitionToState(BoardState state) { StateMachine.TransitionToState(state); }
            public CheckingState(string name, BoardStateMachine stateMachine) : base(name)
            {
                StateMachine = stateMachine;
            }
            public override void OnEnter()
            {

            }

            public override void OnUpdate()
            {
                if (Input.anyKeyDown)
                {
                    StateMachine.TransitionToState(-1);
                    return;
                }
            }

            public override void OnFixedUpdate()
            {

            }

            public override void OnExit()
            {

            }
        }
       

    }
}