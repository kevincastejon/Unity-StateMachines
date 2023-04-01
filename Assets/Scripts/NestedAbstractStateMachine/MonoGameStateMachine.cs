using NestedAbstractStateMachine.BoardSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NestedAbstractStateMachine.GameStateMachine
{
    public enum GameState
    {
        SWITCHING_ROUND = 0,
        PLAYING = 1,
        END = 2,
    }
    public class MonoGameStateMachine : MonoBehaviour
    {
        private GameStateMachine _sm = new GameStateMachine();

        private void Update()
        {
            _sm.Update();
        }
        private void OnGUI()
        {
            // On affiche l'état en cours pour le debug
            GUIStyle style = new GUIStyle() { fontSize = 50, fontStyle = FontStyle.Bold };
            style.normal.textColor = Color.white;
            GUI.Label(new Rect(50, 50, 100, 100), ((GameState)_sm.CurrentState).ToString(), style);
        }
    }
    public class GameStateMachine : NestedAbstractStateMachine
    {
        public GameStateMachine()
        {
            new BoardStateMachine();
            SetStates(new Dictionary<int, AbstractState>() {
                { (int)GameState.SWITCHING_ROUND, new SwitchingRoundState() },
                { (int)GameState.PLAYING, new BoardStateMachine() },
                { (int)GameState.END, new EndState() },
            });
        }

        protected override void ExitFromSubStateMachine(int subStateMachine)
        {
            if (subStateMachine == (int)GameState.PLAYING)
            {
                if (true)
                {

                }
            }
        }
    }
    public class SwitchingRoundState : AbstractState
    {
        public override void OnEnter()
        {

        }

        public override void OnUpdate()
        {
            if (Input.GetButtonDown("Jump"))
            {
                TransitionToState((int)GameState.PLAYING);
            }
        }

        public override void OnExit()
        {

        }
    }
    //public class PlayingState : AbstractState
    //{

    //    public override void OnEnter()
    //    {

    //    }

    //    public override void OnUpdate()
    //    {
    //        if (Input.GetButtonDown("Jump"))
    //        {
    //            TransitionToState((int)GameState.END);
    //        }
    //    }

    //    public override void OnExit()
    //    {

    //    }
    //}
    public class EndState : AbstractState
    {
        public override void OnEnter()
        {

        }

        public override void OnUpdate()
        {
            if (Input.GetButtonDown("Jump"))
            {
                TransitionToState((int)GameState.SWITCHING_ROUND);
            }
        }

        public override void OnExit()
        {

        }
    }
}
