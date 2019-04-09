using static SuperNoughtsAndCrosses.models.PlayerSymbol;

namespace SuperNoughtsAndCrosses.models
{
    public class GameState : IGameState
    {
        private PlayerSymbol _currentPlayerSymbol;
        private readonly PlayerSymbol[][] _board;

        private int _tilesPlayed;
        private bool _isGameOver;
        private GameVictor _victor;

        public GameState()
        {
            _currentPlayerSymbol = CROSS;
            
            _board = new[]
            {
                new[] {UNPLAYED, UNPLAYED, UNPLAYED},
                new[] {UNPLAYED, UNPLAYED, UNPLAYED},
                new[] {UNPLAYED, UNPLAYED, UNPLAYED}
            };

            _tilesPlayed = 0;
            _isGameOver = false;
            _victor = GameVictor.TIE;
        }

        private void ChangeTurn()
        {
            _currentPlayerSymbol = _currentPlayerSymbol == CROSS ? NOUGHT : CROSS;
        }

        public void PlayTile(int row, int col)
        {
            if (!_board[row][col].Equals(UNPLAYED))
            {
                throw new InvalidMoveException();
            }
            
            _board[row][col] = _currentPlayerSymbol;
            _tilesPlayed++;
            
            if (CheckForWin(row, col))
            {
                _isGameOver = true;
                _victor = _currentPlayerSymbol == CROSS ? GameVictor.PLAYER_X : GameVictor.PLAYER_O;
            }

            if (_tilesPlayed == GetNumberOfCols() * GetNumberOfRows())
            {
                _isGameOver = true;
            }

            ChangeTurn();
        }

        private bool CheckForWinOnRow(int row)
        {
            return _board[row].All(tile => tile.Equals(_currentPlayerSymbol));
        }
        
        private bool CheckForWinOnCol(int col)
        {
            return _board.All(r => r[col].Equals(_currentPlayerSymbol));
        }

        private bool CheckForWinDiagonallyDown()
        {
            return Range(0, GetNumberOfRows()).All(i => _board[i][i].Equals(_currentPlayerSymbol));
        }

        private bool CheckForWinDiagonallyUp()
        {
            return Range(0, GetNumberOfRows()).All(i => _board[i][2-i].Equals(_currentPlayerSymbol));
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

        private string Display(PlayerSymbol symbol)
        {
            switch (symbol)
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

        public bool IsGameOver()
        {
            return _isGameOver;
        }

        public GameVictor GetWinner()
        {
            return _victor;
        }
    }
}