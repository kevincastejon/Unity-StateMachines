using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SimpleAbstractStateMachineGenericLess
{

    public class GameStateMachine : AbstractStateMachine
    {
        public enum GameState
        {
            SWITCHING_ROUND,
            PLAYING,
            END
        }
        private void Awake()
        {
            SetStates(new SwitchingRoundState(this), new PlayingState(this), new EndState(this));
            SetDefaultState(GameState.SWITCHING_ROUND);
        }
        private void OnGUI()
        {
            // On affiche l'état en cours pour le debug
            GUIStyle style = new GUIStyle() { fontSize = 50, fontStyle = FontStyle.Bold };
            style.normal.textColor = Color.white;
            GUI.Label(new Rect(50, 50, 100, 100), GetCurrentStateIndex<int>().ToString(), style);
        }
        public class SwitchingRoundState : AbstractState
        {
            private GameStateMachine StateMachine { get => (GameStateMachine)ParentStateMachine; }
            public SwitchingRoundState(AbstractStateMachine parentStateMachine) : base(parentStateMachine)
            {
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


        public class PlayingState : AbstractState
        {
            private GameStateMachine StateMachine { get => (GameStateMachine)ParentStateMachine; }
            public PlayingState(AbstractStateMachine parentStateMachine) : base(parentStateMachine)
            {
            }
            public override void OnEnter()
            {

            }

            public override void OnUpdate()
            {
                if (Input.anyKeyDown)
                {
                    TransitionToState(GameState.END);
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
            private GameStateMachine StateMachine { get => (GameStateMachine)ParentStateMachine; }
            public EndState(AbstractStateMachine parentStateMachine) : base(parentStateMachine)
            {
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