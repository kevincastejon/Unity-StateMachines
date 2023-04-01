using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SimpleAbstractStateMachine
{
    public enum GameState
    {
        SWITCHING_ROUND,
        PLAYING,
        END,
    }
    public class GameStateMachine : AbstractStateMachine<GameState>
    {
        private void Awake()
        {
            SetStates(new Dictionary<GameState, AbstractState<GameState>>() {
            { GameState.SWITCHING_ROUND, new SwitchingRoundState(this) },
            { GameState.PLAYING, new PlayingState(this) },
            { GameState.END, new EndState(this) },
        });
        }
        private void OnGUI()
        {
            // On affiche l'état en cours pour le debug
            GUIStyle style = new GUIStyle() { fontSize = 50, fontStyle = FontStyle.Bold };
            style.normal.textColor = Color.white;
            GUI.Label(new Rect(50, 50, 100, 100), CurrentState.ToString(), style);
        }
    }
    public class SwitchingRoundState : AbstractState<GameState>
    {
        private GameStateMachine StateMachine { get => (GameStateMachine)ParentStateMachine; }
        public SwitchingRoundState(AbstractStateMachine<GameState> parentStateMachine) : base(parentStateMachine)
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
    public class PlayingState : AbstractState<GameState>
    {
        private GameStateMachine StateMachine { get => (GameStateMachine)ParentStateMachine; }
        public PlayingState(AbstractStateMachine<GameState> parentStateMachine) : base(parentStateMachine)
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
    public class EndState : AbstractState<GameState>
    {
        private GameStateMachine StateMachine { get => (GameStateMachine)ParentStateMachine; }
        public EndState(AbstractStateMachine<GameState> parentStateMachine) : base(parentStateMachine)
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