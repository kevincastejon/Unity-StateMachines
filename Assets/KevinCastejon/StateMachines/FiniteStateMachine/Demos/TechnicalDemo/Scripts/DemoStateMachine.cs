using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;
namespace KevinCastejon.FiniteStateMachineDemos.TechnicalDemo
{
    public class DemoStateMachine : AbstractFiniteStateMachine
    {
        private DisplayManager DisplayManager { get; set; }
        public enum DemoState
        {
            A,
            B,
            C
        }
        private void Awake()
        {
            DisplayManager = FindObjectOfType<DisplayManager>();
            Init(DemoState.A,
                AbstractState.Create<AState, DemoState>(DemoState.A, this),
                AbstractState.Create<BState, DemoState>(DemoState.B, this),
                AbstractState.Create<CState, DemoState>(DemoState.C, this)
            );
        }
        public class AState : AbstractState
        {
            public override void OnEnter()
            {
                GetStateMachine<DemoStateMachine>().DisplayManager.EnableA();
            }
            public override void OnUpdate()
            {
                if (Input.anyKeyDown)
                {
                    TransitionToState(DemoState.B);
                }
            }
            public override void OnExit()
            {
                GetStateMachine<DemoStateMachine>().DisplayManager.DisableA();
            }
        }
        public class BState : AbstractState
        {
            public override void OnEnter()
            {
                GetStateMachine<DemoStateMachine>().DisplayManager.EnableB();
            }
            public override void OnUpdate()
            {
                if (Input.anyKeyDown)
                {
                    TransitionToState(DemoState.C);
                }
            }
            public override void OnExit()
            {
                GetStateMachine<DemoStateMachine>().DisplayManager.DisableB();
            }
        }
        public class CState : AbstractState
        {
            public override void OnEnter()
            {
                GetStateMachine<DemoStateMachine>().DisplayManager.EnableC();
            }
            public override void OnUpdate()
            {
                if (Input.anyKeyDown)
                {
                    TransitionToState(DemoState.A);
                }
            }
            public override void OnExit()
            {
                GetStateMachine<DemoStateMachine>().DisplayManager.DisableC();
            }
        }
    }
}
