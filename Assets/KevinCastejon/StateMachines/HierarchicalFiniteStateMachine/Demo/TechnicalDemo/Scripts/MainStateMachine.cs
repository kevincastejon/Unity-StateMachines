using UnityEngine;
using KevinCastejon.HierarchicalFiniteStateMachine;
namespace KevinCastejon.HierarchicalFiniteStateMachineDemos.TechnicalDemo
{
    public class MainStateMachine : AbstractHierarchicalFiniteStateMachine
    {
        private DisplayManager DisplayManager { get; set; }
        public enum MainState
        {
            A,
            B,
            C
        }
        public MainStateMachine()
        {
            Init(MainState.A,
                Create<AState, MainState>(MainState.A, this),
                Create<BStateMachine, MainState>(MainState.B, this),
                Create<CState, MainState>(MainState.C, this)
            );
            DisplayManager = Object.FindObjectOfType<DisplayManager>();
        }
        public override void OnExitFromSubStateMachine(AbstractHierarchicalFiniteStateMachine subStateMachine)
        {
            TransitionToState(MainState.C);
        }
        public class AState : AbstractState
        {
            public override void OnEnter()
            {
                GetStateMachine<MainStateMachine>().DisplayManager.EnableA();
            }
            public override void OnUpdate()
            {
                if (Input.anyKeyDown)
                {
                    TransitionToState(MainState.B);
                }
            }
            public override void OnExit()
            {
                GetStateMachine<MainStateMachine>().DisplayManager.DisableA();
            }
        }
        public class CState : AbstractState
        {
            public override void OnEnter()
            {
                GetStateMachine<MainStateMachine>().DisplayManager.EnableC();
            }
            public override void OnUpdate()
            {
                if (Input.anyKeyDown)
                {
                    TransitionToState(MainState.A);
                }
            }
            public override void OnExit()
            {
                GetStateMachine<MainStateMachine>().DisplayManager.DisableC();
            }
        }
    }
}
