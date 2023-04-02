using UnityEngine;

namespace NestedAbstractStateMachineGenericLess
{
    public class GameStateMachine : AbstractStateMachine
    {
        public enum GameState
        {
            SWITCHING_ROUND,
            PLAYING,
            END
        }
        public GameStateMachine() : base("GameStateMachine")
        {
            Init(GameState.SWITCHING_ROUND, 
                new SwitchingRoundState(GameState.SWITCHING_ROUND.ToString(), this), 
                new BoardStateMachine(GameState.PLAYING.ToString(), this), 
                new EndState(GameState.END.ToString(), this)
            );
        }

        public override void OnExitFromSubStateMachine(AbstractStateMachine subStateMachine)
        {
            TransitionToState(GameState.END);
        }

        public class SwitchingRoundState : AbstractState
        {
            private GameStateMachine StateMachine { get; set; }
            private void TransitionToState(GameState state) { StateMachine.TransitionToState(state); }
            public SwitchingRoundState(string name, GameStateMachine stateMachine) : base(name)
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
                    TransitionToState(GameState.PLAYING);
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

        public class EndState : AbstractState
        {
            private GameStateMachine StateMachine { get; set; }
            private void TransitionToState(GameState state) { StateMachine.TransitionToState(state); }
            public EndState(string name, GameStateMachine stateMachine) : base(name)
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
                    TransitionToState(GameState.SWITCHING_ROUND);
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