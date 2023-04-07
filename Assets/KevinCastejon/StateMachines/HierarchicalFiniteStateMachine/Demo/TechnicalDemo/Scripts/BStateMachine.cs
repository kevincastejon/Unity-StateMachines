using KevinCastejon.HierarchicalFiniteStateMachine;
using UnityEngine;

namespace KevinCastejon.HierarchicalFiniteStateMachineDemos.TechnicalDemo
{
    public class BStateMachine : AbstractHierarchicalFiniteStateMachine
    {
        private DisplayManager DisplayManager { get; set; }
        public enum SubState
        {
            SUB_A,
            SUB_B,
            SUB_C
        }
        public BStateMachine()
        {
            Init(SubState.SUB_A,
                Create<SubAState, SubState>(SubState.SUB_A, this),
                Create<SubBState, SubState>(SubState.SUB_B, this),
                Create<SubCState, SubState>(SubState.SUB_C, this)
            );
            DisplayManager = Object.FindObjectOfType<DisplayManager>();
        }
        public override void OnStateMachineEntry()
        {
            DisplayManager.EnableB();
        }
        public override void OnStateMachineExit()
        {
            DisplayManager.DisableB();
        }
        public class SubAState : AbstractState
        {
            public override void OnEnter()
            {
                GetStateMachine<BStateMachine>().DisplayManager.EnableSubA();
            }
            public override void OnUpdate()
            {
                if (Input.anyKeyDown)
                {
                    TransitionToState(SubState.SUB_B);
                }
            }
            public override void OnExit()
            {
                GetStateMachine<BStateMachine>().DisplayManager.DisableSubA();
            }
        }
        public class SubBState : AbstractState
        {
            public override void OnEnter()
            {
                GetStateMachine<BStateMachine>().DisplayManager.EnableSubB();
            }
            public override void OnUpdate()
            {
                if (Input.anyKeyDown)
                {
                    TransitionToState(SubState.SUB_C);
                }
            }
            public override void OnExit()
            {
                GetStateMachine<BStateMachine>().DisplayManager.DisableSubB();
            }
        }
        public class SubCState : AbstractState
        {
            public override void OnEnter()
            {
                GetStateMachine<BStateMachine>().DisplayManager.EnableSubC();
            }
            public override void OnUpdate()
            {
                if (Input.anyKeyDown)
                {
                    TransitionToState(EXIT);
                }
            }
            public override void OnExit()
            {
                GetStateMachine<BStateMachine>().DisplayManager.DisableSubC();
            }
        }
    }
}
