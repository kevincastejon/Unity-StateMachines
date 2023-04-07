using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KevinCastejon.HierarchicalFiniteStateMachineDemos.TicTacToeDemo
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _startPanel;
        [SerializeField] private GameObject _backPanel;
        [SerializeField] private GameObject _playerAPanel;
        [SerializeField] private GameObject _playerBPanel;
        [SerializeField] private GameObject _boardGameObject;
        [SerializeField] private GameObject _playerAWinPanel;
        [SerializeField] private GameObject _playerBWinPanel;
        [SerializeField] private GameObject _drawPanel;
        [SerializeField] private Tile[] _line1;
        [SerializeField] private Tile[] _line2;
        [SerializeField] private Tile[] _line3;
        private Vector2Int _lastMoveTile;
        private bool _lastMoveValue;
        private bool _playerA;
        private bool _isAnyWinner;
        private int _moveLeft = 9;
        private readonly Tile[,] _board = new Tile[3, 3];

        public bool IsMoveLeft { get => _moveLeft > 0; }
        public bool IsAnyWinner { get => _isAnyWinner; }

        private void Awake()
        {
            for (int i = 0; i < 3; i++)
            {
                _board[0, i] = _line1[i];
                _board[1, i] = _line2[i];
                _board[2, i] = _line3[i];
            }
        }
        public void ShowStartPanel()
        {
            _startPanel.SetActive(true);
        }
        public void HideStartPanel()
        {
            _startPanel.SetActive(false);
        }
        public void ShowBackPanel()
        {
            _backPanel.SetActive(true);
        }
        public void HideBackPanel()
        {
            _backPanel.SetActive(false);
        }
        public void ShowBoard()
        {
            _boardGameObject.SetActive(true);
        }
        public void HideBoard()
        {
            _boardGameObject.SetActive(false);
        }
        public void SwitchPlayer()
        {
            _playerA = !_playerA;
            _playerAPanel.SetActive(_playerA);
            _playerBPanel.SetActive(!_playerA);
        }
        public bool DetectTileClick()
        {
            if (Input.GetMouseButton(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                if (tile.State == TileState.EMPTY)
                {
                    if (_playerA)
                    {
                        tile.State = TileState.X;
                    }
                    else
                    {
                        tile.State = TileState.O;
                    }
                    _lastMoveTile = new Vector2Int(tile.X, tile.Y);
                    _lastMoveValue = _playerA;
                    _moveLeft--;

                    return true;
                }
            }
            return false;
        }
        public bool IsVictory()
        {
            TileState state = _lastMoveValue ? TileState.X : TileState.O;
            for (int i = 0; i < 3; i++)
            {
                if (_board[_lastMoveTile.y, i].State != state)
                {
                    break;
                }
                if (i == 2)
                {
                    _isAnyWinner = true;
                    return true;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                if (_board[i, _lastMoveTile.x].State != state)
                {
                    break;
                }
                if (i == 2)
                {
                    _isAnyWinner = true;
                    return true;
                }
            }
            if (_lastMoveTile.x == _lastMoveTile.y)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (_board[i, i].State != state)
                    {
                        break;
                    }
                    if (i == 2)
                    {
                        _isAnyWinner = true;
                        return true;
                    }
                }
            }
            if (_lastMoveTile.x + _lastMoveTile.y == 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (_board[i, 2 - i].State != state)
                    {
                        break;
                    }
                    if (i == 2)
                    {
                        _isAnyWinner = true;
                        return true;
                    }
                }
            }
            return false;
        }
        public void ShowVictory()
        {
            if (_lastMoveValue)
            {
                _playerAWinPanel.SetActive(true);
            }
            else
            {
                _playerBWinPanel.SetActive(true);
            }
        }
        public void ShowDraw()
        {
            _drawPanel.SetActive(true);
        }
        public void HideCurrentPlayerPanel()
        {
            _playerAPanel.SetActive(false);
            _playerBPanel.SetActive(false);
        }
        public void ResetGame()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _board[i, j].State = TileState.EMPTY;
                    _board[i, j].X = j;
                    _board[i, j].Y = i;
                }
            }
            _playerAWinPanel.SetActive(false);
            _playerBWinPanel.SetActive(false);
            _drawPanel.SetActive(false);
            _playerA = false;
            _isAnyWinner = false;
            _moveLeft = 9;
        }
    }
}