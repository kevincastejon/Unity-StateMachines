using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;
namespace KevinCastejon.FiniteStateMachineDemos.TicTacToeDemo
{
    public class GameStateMachine : AbstractFiniteStateMachine
    {
        public GameManager Manager { get; set; }
        public enum GameState
        {
            MENU,
            SWITCHING_PLAYER,
            PICKING_TILE,
            CHECKING_VICTORY,
            END,
        }
        private void Awake()
        {
            Init(GameState.SWITCHING_PLAYER,
                AbstractState.Create<MenuState, GameState>(GameState.MENU, this),
                AbstractState.Create<SwitchingState, GameState>(GameState.SWITCHING_PLAYER, this),
                AbstractState.Create<PickingState, GameState>(GameState.PICKING_TILE, this),
                AbstractState.Create<CheckingState, GameState>(GameState.CHECKING_VICTORY, this),
                AbstractState.Create<EndState, GameState>(GameState.END, this)
            );
            Manager = FindObjectOfType<GameManager>();
        }
        public class MenuState : AbstractState
        {
            public override void OnEnter()
            {
                GetStateMachine<GameStateMachine>().Manager.ResetGame();
                GetStateMachine<GameStateMachine>().Manager.ShowStartPanel();
            }

            public override void OnUpdate()
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    TransitionToState(GameState.SWITCHING_PLAYER);
                    return;
                }
            }

            public override void OnExit()
            {
                GetStateMachine<GameStateMachine>().Manager.HideStartPanel();
                GetStateMachine<GameStateMachine>().Manager.ResetGame();
                GetStateMachine<GameStateMachine>().Manager.ShowBoard();
            }
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
                    GetStateMachine<GameStateMachine>().TransitionToState(GameState.END);
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

            public override void OnExit()
            {

            }
        }
        public class EndState : AbstractState
        {
            public override void OnEnter()
            {
                GetStateMachine<GameStateMachine>().Manager.HideBoard();
                GetStateMachine<GameStateMachine>().Manager.HideCurrentPlayerPanel();
                GetStateMachine<GameStateMachine>().Manager.ShowBackPanel();
                if (GetStateMachine<GameStateMachine>().Manager.IsAnyWinner)
                {
                    GetStateMachine<GameStateMachine>().Manager.ShowVictory();
                }
                else
                {
                    GetStateMachine<GameStateMachine>().Manager.ShowDraw();
                }
            }

            public override void OnUpdate()
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    TransitionToState(GameState.MENU);
                    return;
                }
            }

            public override void OnExit()
            {
                GetStateMachine<GameStateMachine>().Manager.HideBackPanel();
            }
        }
    }
}