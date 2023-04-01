using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NestedAbstractStateMachine.BoardSM
{
    public enum BoardState
    {
        SWITCHING_PLAYER,
        PLAYING,
        CHECKING,
    }
    public class BoardStateMachine : NestedAbstractStateMachine
    {
        public BoardStateMachine()
        {
            SetStates(new Dictionary<int, AbstractState>() {
                { (int)BoardState.SWITCHING_PLAYER, new SwitchingPlayerState() },
                { (int)BoardState.PLAYING, new PlayingState() },
                { (int)BoardState.CHECKING, new CheckingState() },
            });
        }

        protected override void ExitFromSubStateMachine(int subStateMachine)
        {

        }
    }
    public class SwitchingPlayerState : AbstractState
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
    public class PlayingState : AbstractState
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
    public class CheckingState : AbstractState
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
