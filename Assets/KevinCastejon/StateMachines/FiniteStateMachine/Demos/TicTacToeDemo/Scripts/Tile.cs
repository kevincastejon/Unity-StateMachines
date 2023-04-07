using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KevinCastejon.FiniteStateMachineDemos.TicTacToeDemo
{
    public enum TileState
    {
        EMPTY,
        X,
        O
    }

    public class Tile : MonoBehaviour
    {
        [SerializeField] private GameObject _X;
        [SerializeField] private GameObject _O;
        private TileState _state;
        private int _x;
        private int _y;
        public TileState State
        {
            get
            {
                return _state;
            }

            set
            {
                _state = value;
                if (_state == TileState.X)
                {
                    _X.SetActive(true);
                }
                else if (_state == TileState.O)
                {
                    _O.SetActive(true);
                }
                else if (_state == TileState.EMPTY)
                {
                    _X.SetActive(false);
                    _O.SetActive(false);
                }
            }
        }

        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
    }
}
