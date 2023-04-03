using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NestedAbstractStateMachineGenericLess
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
        private GameStateMachine _sm;
        private Vector2Int _lastMoveTile;
        private bool _lastMoveValue;
        private bool _playerA;
        private bool _isWinner;
        private int _moveLeft = 9;
        private readonly Tile[,] _board = new Tile[3, 3];

        public int MoveLeft { get => _moveLeft; }
        public bool IsWinner { get => _isWinner; }

        private void Awake()
        {
            _sm = AbstractStateMachine.Create<GameStateMachine>("GameStateMachine");
            for (int i = 0; i < 3; i++)
            {
                _board[0, i] = _line1[i];
                _board[1, i] = _line2[i];
                _board[2, i] = _line3[i];
            }
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
        public Tile GetClickedTile()
        {
            if (Input.GetMouseButton(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                if (tile.State == TileState.EMPTY)
                {
                    return tile;
                }
            }
            return null;
        }
        public void PlayTile(Tile tile)
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
                    _isWinner = true;
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
                    _isWinner = true;
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
                        _isWinner = true;
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
                        _isWinner = true;
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
            _isWinner = false;
            _moveLeft = 9;
        }
    }
}