using static SuperNoughtsAndCrosses.models.PlayerSymbol;

namespace SuperNoughtsAndCrosses.models
{
    public class GameState : IGameState
    {
        private PlayerSymbol _currentPlayerSymbol;
        private readonly PlayerSymbol[][] _board;

        public GameState()
        {
            _currentPlayerSymbol = CROSS;
            _board = new[]
            {
                new[] {UNPLAYED, UNPLAYED, UNPLAYED},
                new[] {UNPLAYED, UNPLAYED, UNPLAYED},
                new[] {UNPLAYED, UNPLAYED, UNPLAYED}
            };
        }

        private void ChangeTurn()
        {
            _currentPlayerSymbol = _currentPlayerSymbol == CROSS ? NOUGHT : CROSS;
        }

        public void PlayTile(int row, int col)
        {
            _board[row][col] = _currentPlayerSymbol;
            ChangeTurn();
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
    }
}