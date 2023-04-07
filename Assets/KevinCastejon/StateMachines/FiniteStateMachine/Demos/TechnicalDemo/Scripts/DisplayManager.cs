using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace KevinCastejon.FiniteStateMachineDemos.TechnicalDemo
{
    public class DisplayManager : MonoBehaviour
    {
        [SerializeField] private Renderer _A;
        [SerializeField] private Renderer _B;
        [SerializeField] private Renderer _C;
        public void EnableA()
        {
            _A.material.color = Color.blue;
        }
        public void DisableA()
        {
            _A.material.color = Color.white;
        }
        public void EnableB()
        {
            _B.material.color = Color.blue;
        }
        public void DisableB()
        {
            _B.material.color = Color.white;
        }
        public void EnableC()
        {
            _C.material.color = Color.blue;
        }
        public void DisableC()
        {
            _C.material.color = Color.white;
        }
    }
}
