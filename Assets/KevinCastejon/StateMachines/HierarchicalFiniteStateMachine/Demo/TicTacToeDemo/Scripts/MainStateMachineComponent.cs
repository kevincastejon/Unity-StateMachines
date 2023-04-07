using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.HierarchicalFiniteStateMachine;
namespace KevinCastejon.HierarchicalFiniteStateMachineDemos.TicTacToeDemo
{
    public class MainStateMachineComponent : MonoBehaviour
    {
        private MainStateMachine _sm;
        private void Awake()
        {
            _sm = AbstractHierarchicalFiniteStateMachine.CreateRootStateMachine<MainStateMachine>("MainStateMachine");
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
    }
}
