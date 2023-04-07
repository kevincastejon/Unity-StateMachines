using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.HierarchicalFiniteStateMachine;
namespace KevinCastejon.HierarchicalFiniteStateMachineDemos.TicTacToeDemo
{
    public class GameStateMachine : AbstractHierarchicalFiniteStateMachine
    {
        public GameManager Manager { get; set; }
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
                GetStateMachine<GameStateMachine>().Manager.SwitchPlayer();
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
                if (GetStateMachine<GameStateMachine>().Manager.DetectTileClick())
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
                if (GetStateMachine<GameStateMachine>().Manager.IsVictory() || !GetStateMachine<GameStateMachine>().Manager.IsMoveLeft)
                {
                    GetStateMachine<GameStateMachine>().TransitionToState(EXIT);
                    return;
                }
                else
                {
                    GetStateMachine<GameStateMachine>().TransitionToState(GameState.SWITCHING_PLAYER);
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