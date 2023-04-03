using UnityEngine;

namespace NestedAbstractStateMachineGenericLess
{
    public class GameStateMachine : AbstractStateMachine
    {
        public GameManager Manager { get; set; }
        public enum GameState
        {
            MENU,
            GAME,
            END
        }
        public GameStateMachine()
        {
            Init(GameState.MENU,
                Create<MenuState, GameState>(GameState.MENU, this),
                Create<BoardStateMachine, GameState>(GameState.GAME, this),
                Create<EndState, GameState>(GameState.END, this)
            );
            Manager = Object.FindObjectOfType<GameManager>();
        }

        public override void OnExitFromSubStateMachine(AbstractStateMachine subStateMachine)
        {
            TransitionToState(GameState.END);
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
                    GetStateMachine<GameStateMachine>().TransitionToState(GameState.GAME);
                    return;
                }
            }

            public override void OnExit()
            {
                GetStateMachine<GameStateMachine>().Manager.HideStartPanel();
            }
        }

        public class EndState : AbstractState
        {
            public override void OnEnter()
            {
                GetStateMachine<GameStateMachine>().Manager.HideCurrentPlayerPanel();
                GetStateMachine<GameStateMachine>().Manager.ShowBackPanel();
                if (GetStateMachine<GameStateMachine>().Manager.IsWinner)
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
                    GetStateMachine<GameStateMachine>().TransitionToState(GameState.MENU);
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