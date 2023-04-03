using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NestedAbstractStateMachineGenericLess
{
    public class BoardStateMachine : AbstractStateMachine
    {
        public GameManager Manager { get; set; }
        public enum BoardState
        {
            SWITCHING_PLAYER,
            PICKING_TILE,
            CHECKING_VICTORY,
        }
        public BoardStateMachine()
        {
            Init(BoardState.SWITCHING_PLAYER,
                Create<SwitchingState, BoardState>(BoardState.SWITCHING_PLAYER, this),
                Create<PickingState, BoardState>(BoardState.PICKING_TILE, this),
                Create<CheckingState, BoardState>(BoardState.CHECKING_VICTORY, this)
            );
            Manager = Object.FindObjectOfType<GameManager>();
        }
        public override void OnStateMachineEntry()
        {
            Manager.ResetGame();
            Manager.ShowBoard();
        }
        public override void OnStateMachineExit()
        {
            Manager.HideBoard();
        }
        public class SwitchingState : AbstractState
        {
            public override void OnEnter()
            {
                GetStateMachine<BoardStateMachine>().Manager.SwitchPlayer();
            }

            public override void OnUpdate()
            {
                GetStateMachine<BoardStateMachine>().TransitionToState(BoardState.PICKING_TILE);
            }

            public override void OnFixedUpdate()
            {

            }

            public override void OnExit()
            {

            }
        }


        public class PickingState : AbstractState
        {
            public override void OnEnter()
            {

            }

            public override void OnUpdate()
            {
                Tile clickedTile = GetStateMachine<BoardStateMachine>().Manager.GetClickedTile();
                if (clickedTile)
                {
                    GetStateMachine<BoardStateMachine>().Manager.PlayTile(clickedTile);
                    GetStateMachine<BoardStateMachine>().TransitionToState(BoardState.CHECKING_VICTORY);
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
            public override void OnEnter()
            {
                if (GetStateMachine<BoardStateMachine>().Manager.IsVictory() || GetStateMachine<BoardStateMachine>().Manager.MoveLeft == 0)
                {
                    GetStateMachine<BoardStateMachine>().TransitionToState(EXIT);
                    return;
                }
                else
                {
                    GetStateMachine<BoardStateMachine>().TransitionToState(BoardState.SWITCHING_PLAYER);
                    return;
                }
            }

            public override void OnUpdate()
            {
                
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