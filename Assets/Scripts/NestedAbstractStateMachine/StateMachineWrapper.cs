using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NestedAbstractStateMachineGenericLess
{
    public class StateMachineWrapper : MonoBehaviour
    {
        private GameStateMachine _sm;

        private void Awake()
        {
            _sm = new GameStateMachine();
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
            GUI.Label(new Rect(50, 50, 100, 100), _sm.GetCurrentHierarchicalStatesNamesString(), style);
        }
    }
}