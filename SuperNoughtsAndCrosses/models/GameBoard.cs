using static SuperNoughtsAndCrosses.models.Player;

namespace SuperNoughtsAndCrosses.models
{
    public class GameBoard : AbstractGameBoard<Player>
    {
        private int _tilesPlayed;

        public GameBoard()
        {
            _tilesPlayed = 0;
        }
        
        public void PlayTile(int row, int col, Player player)
        {
            if (!Board[row][col].Equals(NONE))
            {
                throw new InvalidMoveException();
            }
            
            Board[row][col] = player;
            _tilesPlayed++;
            
            if (CheckForWin(row, col, player))
            {
                _isGameOver = true;
                Winner = player;
            }

            if (_tilesPlayed == Size * Size)
            {
                _isGameOver = true;
            }
        }

        protected override bool CheckPlayerWinsTile(Player tilePlayer, Player currentPlayer)
        {
            return tilePlayer.Equals(currentPlayer);
        }

        protected override Player GetEmptyTile()
        {
            return NONE;
        }

        public string GetSymbolForTile(int row, int col)
        {
            return Display(Board[row][col]);
        }
    }
}