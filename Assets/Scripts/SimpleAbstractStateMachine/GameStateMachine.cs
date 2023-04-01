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
            { GameState.SWITCHING_ROUND, new SwitchingRoundState() },
            { GameState.PLAYING, new PlayingState() },
            { GameState.END, new EndState() },
        });
        }
    }

    public class SwitchingRoundState : AbstractState<GameState>
    {
        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }

        public override void OnUpdate()
        {

        }
    }
    public class PlayingState : AbstractState<GameState>
    {

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }

        public override void OnUpdate()
        {

        }
    }
    public class EndState : AbstractState<GameState>
    {
        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }

        public override void OnUpdate()
        {

        }
    }
}