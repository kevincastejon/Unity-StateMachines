using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.HierarchicalFiniteStateMachine;
namespace KevinCastejon.HierarchicalFiniteStateMachineDemos.TicTacToeDemo
{
    public class MainStateMachine : AbstractHierarchicalFiniteStateMachine
    {
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
        }

        public override void OnExitFromSubStateMachine(AbstractHierarchicalFiniteStateMachine subStateMachine)
        {
            TransitionToState(MainState.END);
        }
        
        public class MenuState : AbstractState 
        {
            public override void OnEnter()
            {
                (RootComponent as MainStateMachineComponent).gameManager.ResetGame();
                (RootComponent as MainStateMachineComponent).gameManager.ShowStartPanel();
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
                (RootComponent as MainStateMachineComponent).gameManager.HideStartPanel();
            }
        }

        public class EndState : AbstractState
        {
            public override void OnEnter()
            {
                (RootComponent as MainStateMachineComponent).gameManager.HideCurrentPlayerPanel();
                (RootComponent as MainStateMachineComponent).gameManager.ShowBackPanel();
                if ((RootComponent as MainStateMachineComponent).gameManager.IsAnyWinner)
                {
                    (RootComponent as MainStateMachineComponent).gameManager.ShowVictory();
                }
                else
                {
                    (RootComponent as MainStateMachineComponent).gameManager.ShowDraw();
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
                (RootComponent as MainStateMachineComponent).gameManager.HideBackPanel();
            }
        }
    }
}