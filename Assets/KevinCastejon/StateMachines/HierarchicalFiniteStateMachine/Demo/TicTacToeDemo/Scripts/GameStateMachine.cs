using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.HierarchicalFiniteStateMachine;
namespace KevinCastejon.HierarchicalFiniteStateMachineDemos.TicTacToeDemo
{
    public class GameStateMachine : AbstractHierarchicalFiniteStateMachine
    {
        public enum GameState
        {
            SWITCHING_PLAYER,
            PICKING_TILE,
            CHECKING_VICTORY,
        }
        public GameStateMachine()
        {
            Init(GameState.SWITCHING_PLAYER,
                Create<SwitchingState, GameState>(GameState.SWITCHING_PLAYER, this),
                Create<PickingState, GameState>(GameState.PICKING_TILE, this),
                Create<CheckingState, GameState>(GameState.CHECKING_VICTORY, this)
            );
        }
        public override void OnStateMachineEntry()
        {
            (RootComponent as MainStateMachineComponent).gameManager.ResetGame();
            (RootComponent as MainStateMachineComponent).gameManager.ShowBoard();
        }
        public override void OnStateMachineExit()
        {
            (RootComponent as MainStateMachineComponent).gameManager.HideBoard();
        }
        public class SwitchingState : AbstractState
        {
            public override void OnEnter()
            {
                (RootComponent as MainStateMachineComponent).gameManager.SwitchPlayer();
            }

            public override void OnUpdate()
            {
                TransitionToState(GameState.PICKING_TILE);
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
                if ((RootComponent as MainStateMachineComponent).gameManager.DetectTileClick())
                {
                    TransitionToState(GameState.CHECKING_VICTORY);
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
                if ((RootComponent as MainStateMachineComponent).gameManager.IsVictory() || !(RootComponent as MainStateMachineComponent).gameManager.IsMoveLeft)
                {
                    TransitionToState(EXIT);
                    return;
                }
                else
                {
                    TransitionToState(GameState.SWITCHING_PLAYER);
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