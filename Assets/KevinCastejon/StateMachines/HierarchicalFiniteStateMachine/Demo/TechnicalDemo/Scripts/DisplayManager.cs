using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace KevinCastejon.HierarchicalFiniteStateMachineDemos.TechnicalDemo
{
    public class DisplayManager : MonoBehaviour
    {
        [SerializeField] private Renderer _A;
        [SerializeField] private Renderer _B;
        [SerializeField] private Renderer _C;
        [SerializeField] private Renderer _SubA;
        [SerializeField] private Renderer _SubB;
        [SerializeField] private Renderer _SubC;
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
        public void EnableSubA()
        {
            _SubA.material.color = Color.cyan;
        }
        public void DisableSubA()
        {
            _SubA.material.color = Color.white;
        }
        public void EnableSubB()
        {
            _SubB.material.color = Color.cyan;
        }
        public void DisableSubB()
        {
            _SubB.material.color = Color.white;
        }
        public void EnableSubC()
        {
            _SubC.material.color = Color.cyan;
        }
        public void DisableSubC()
        {
            _SubC.material.color = Color.white;
        }
    }
}
