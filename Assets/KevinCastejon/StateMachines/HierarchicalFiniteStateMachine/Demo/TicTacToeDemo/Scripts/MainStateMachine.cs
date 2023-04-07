using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.HierarchicalFiniteStateMachine;
namespace KevinCastejon.HierarchicalFiniteStateMachineDemos.TicTacToeDemo
{
    public class MainStateMachine : AbstractHierarchicalFiniteStateMachine
    {
        public GameManager Manager { get; set; }
        public enum MainState
        {
            MENU,
            GAME,
            END,
        }
        public MainStateMachine()
        {
            Init(MainState.MENU,
                Create<MenuState, MainState>(MainState.MENU, this),
                Create<GameStateMachine, MainState>(MainState.GAME, this),
                Create<EndState, MainState>(MainState.END, this)
            );
            Manager = Object.FindObjectOfType<GameManager>();
        }

        public override void OnExitFromSubStateMachine(AbstractHierarchicalFiniteStateMachine subStateMachine)
        {
            TransitionToState(MainState.END);
        }
        
        public class MenuState : AbstractState
        {
            public override void OnEnter()
            {
                GetStateMachine<MainStateMachine>().Manager.ResetGame();
                GetStateMachine<MainStateMachine>().Manager.ShowStartPanel();
            }

            public override void OnUpdate()
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    TransitionToState(MainState.GAME);
                    return;
                }
            }

            public override void OnExit()
            {
                GetStateMachine<MainStateMachine>().Manager.HideStartPanel();
            }
        }

        public class EndState : AbstractState
        {
            public override void OnEnter()
            {
                GetStateMachine<MainStateMachine>().Manager.HideCurrentPlayerPanel();
                GetStateMachine<MainStateMachine>().Manager.ShowBackPanel();
                if (GetStateMachine<MainStateMachine>().Manager.IsAnyWinner)
                {
                    GetStateMachine<MainStateMachine>().Manager.ShowVictory();
                }
                else
                {
                    GetStateMachine<MainStateMachine>().Manager.ShowDraw();
                }
            }

            public override void OnUpdate()
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    TransitionToState(MainState.MENU);
                    return;
                }
            }

            public override void OnExit()
            {
                GetStateMachine<MainStateMachine>().Manager.HideBackPanel();
            }
        }
    }
}