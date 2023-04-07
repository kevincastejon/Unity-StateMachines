using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.HierarchicalFiniteStateMachine;
namespace KevinCastejon.HierarchicalFiniteStateMachineDemos.TechnicalDemo
{
    public class MainStateMachineComponent : MonoBehaviour
    {
        private MainStateMachine _stateMachine;
        private void Awake()
        {
            _stateMachine = AbstractHierarchicalFiniteStateMachine.CreateRootStateMachine<MainStateMachine>("MainStateMachine");
        }
        private void Start()
        {
            _stateMachine.OnEnter();
        }
        private void Update()
        {
            _stateMachine.OnUpdate();
        }
    }
}
