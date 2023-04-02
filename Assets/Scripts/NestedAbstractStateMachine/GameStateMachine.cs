using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NestedAbstractStateMachineGenericLess
{
    public class GameStateMachine : MonoBehaviour
    {
        private _GameStateMachine _sm;

        private void Awake()
        {
            _sm = new _GameStateMachine();
        }

        private void Start()
        {
            _sm.OnEnter();
        }
        private void Update()
        {
            _sm.OnUpdate();
        }
        private void FixedUpdate()
        {
            _sm.OnFixedUpdate();            
        }
        private void OnGUI()
        {
            // On affiche l'état en cours pour le debug
            GUIStyle style = new GUIStyle() { fontSize = 50, fontStyle = FontStyle.Bold };
            style.normal.textColor = Color.white;
            style.fontSize = 10;
            GUI.Label(new Rect(50, 50, 100, 100), string.Join(',', _sm.GetCurrentHierarchicalStates<IAbstractState>().Select(s => s.ToString()).ToArray()), style);
        }
    }
    public class _GameStateMachine : AbstractStateMachine
    {
        public _GameStateMachine()
        {
            Init(null, GameState.SWITCHING_ROUND, new SwitchingRoundState(this), new _BoardStateMachine(this), new EndState(this));
        }

        public override void OnExitFromSubStateMachine(AbstractStateMachine subStateMachine)
        {
            TransitionToState(GameState.END);
        }

        public enum GameState
        {
            SWITCHING_ROUND,
            PLAYING,
            END
        }
        
        public class SwitchingRoundState : IAbstractState
        {
            private _GameStateMachine StateMachine { get; set; }
            private void TransitionToState(GameState state) { StateMachine.TransitionToState(state); }
            public SwitchingRoundState(_GameStateMachine stateMachine)
            {
                StateMachine = stateMachine;
            }

            public void OnEnter()
            {

            }

            public void OnUpdate()
            {
                if (Input.anyKeyDown)
                {
                    TransitionToState(GameState.PLAYING);
                    return;
                }
            }

            public void OnFixedUpdate()
            {

            }

            public void OnExit()
            {

            }
        }


        //public class PlayingState : IAbstractState
        //{
        //    private _GameStateMachine StateMachine { get; set; }
        //    private void TransitionToState(GameState state) { StateMachine.TransitionToState(state); }
        //    public PlayingState(_GameStateMachine stateMachine)
        //    {
        //        StateMachine = stateMachine;
        //    }
        //    public void OnEnter()
        //    {

        //    }

        //    public void OnUpdate()
        //    {
        //        if (Input.anyKeyDown)
        //        {
        //            TransitionToState(GameState.END);
        //            return;
        //        }
        //    }

        //    public void OnFixedUpdate()
        //    {

        //    }

        //    public void OnExit()
        //    {

        //    }
        //}
        public class EndState : IAbstractState
        {
            private _GameStateMachine StateMachine { get; set; }
            private void TransitionToState(GameState state) { StateMachine.TransitionToState(state); }
            public EndState(_GameStateMachine stateMachine)
            {
                StateMachine = stateMachine;
            }
            public void OnEnter()
            {

            }

            public void OnUpdate()
            {
                if (Input.anyKeyDown)
                {
                    TransitionToState(GameState.SWITCHING_ROUND);
                    return;
                }
            }

            public void OnFixedUpdate()
            {

            }

            public void OnExit()
            {

            }
        }
    }
}