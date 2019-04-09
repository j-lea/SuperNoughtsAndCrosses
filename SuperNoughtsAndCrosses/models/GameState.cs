using System.Linq;
using static System.Linq.Enumerable;
using static SuperNoughtsAndCrosses.models.Player;

namespace SuperNoughtsAndCrosses.models
{
    public class GameState : IGameState
    {
        private const int Size = 3;
        
        private Player _currentPlayer;
        private readonly Player[][] _board;

        private int _tilesPlayed;
        private bool _isGameOver;
        private Player _winner;

        public GameState()
        {
            _currentPlayer = CROSS;

            _board = GetBoardOfSize(); 
            _tilesPlayed = 0;
            _isGameOver = false;
            _winner = NONE;
        }

        private Player[][] GetBoardOfSize()
        {
            return Range(0, Size).Select(rows => Range(0, Size).Select(cols => NONE).ToArray()).ToArray();
        }

        private void ChangeTurn()
        {
            _currentPlayer = _currentPlayer == CROSS ? NOUGHT : CROSS;
        }

        public void PlayTile(int row, int col)
        {
            if (!_board[row][col].Equals(NONE))
            {
                throw new InvalidMoveException();
            }
            
            _board[row][col] = _currentPlayer;
            _tilesPlayed++;
            
            if (CheckForWin(row, col))
            {
                _isGameOver = true;
                _winner = _currentPlayer;
            }

            if (_tilesPlayed == GetNumberOfCols() * GetNumberOfRows())
            {
                _isGameOver = true;
            }

            ChangeTurn();
        }

        private bool CheckForWinOnRow(int row)
        {
            return _board[row].All(tile => tile.Equals(_currentPlayer));
        }
        
        private bool CheckForWinOnCol(int col)
        {
            return _board.All(r => r[col].Equals(_currentPlayer));
        }

        private bool CheckForWinDiagonallyDown()
        {
            return Range(0, Size).All(i => _board[i][i].Equals(_currentPlayer));
        }

        private bool CheckForWinDiagonallyUp()
        {
            var maxTileIndex = Size - 1; 
            return Range(0, GetNumberOfRows()).All(i => _board[i][maxTileIndex-i].Equals(_currentPlayer));
        }

        private bool CheckForWin(int row, int col)
        {
            return CheckForWinOnRow(row) || 
                   CheckForWinOnCol(col) || 
                   CheckForWinDiagonallyUp() ||
                   CheckForWinDiagonallyDown();
        }

        public int GetNumberOfRows()
        {
            return _board.Length;
        }

        public int GetNumberOfCols()
        {
            return _board[0].Length;
        }

        public string Display(Player player)
        {
            switch (player)
            {
                case NOUGHT:
                    return "O";
                case CROSS:
                    return "X";
                default:
                    return "";
            }
        }
        
        public string GetSymbolForTile(int row, int col)
        {
            return Display(_board[row][col]);
        }

        public Player GetWinner()
        {
            return _winner;
        }

        public bool IsGameOver()
        {
            return _isGameOver;
        }
    }
}