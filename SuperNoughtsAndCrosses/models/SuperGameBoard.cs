using System;
using static SuperNoughtsAndCrosses.models.Player;

namespace SuperNoughtsAndCrosses.models
{
    public class SuperGameBoard : AbstractGameBoard<GameBoard>
    {
        private Player _currentPlayer;
        private Tuple<int, int> _nextBoardPos;

        public SuperGameBoard()
        {
            _currentPlayer = CROSS;
            _nextBoardPos = null;
        }
        
        private void ChangeTurn()
        {
            _currentPlayer = _currentPlayer == CROSS ? NOUGHT : CROSS;
        }

        public GameBoard GetBoardAtPosition(int row, int col)
        {
            return Board[row][col];
        }
        
        public void PlayFirstMove(int boardRow, int boardCol, int tileRow, int tileCol)
        {
            PlayTileOnBoard(boardRow, boardCol, tileRow, tileCol);
        }

        public void PlayTile(int tileRow, int tileCol)
        {
            var boardRow = _nextBoardPos.Item1;
            var boardCol = _nextBoardPos.Item2;
            
            PlayTileOnBoard(boardRow, boardCol, tileRow, tileCol);
            
            if (GameOverOnBoard(boardRow, boardCol))
            {
                var boardWinner = GetWinnerOfBoard(boardRow, boardCol);

                if (CheckForWin(boardRow, boardCol, boardWinner))
                {
                    _isGameOver = true;
                    Winner = boardWinner;
                }
            }
        }

        protected override bool CheckPlayerWinsTile(GameBoard board, Player player)
        {
            return board.IsGameOver() && board.GetWinner().Equals(player);
        }

        private void PlayTileOnBoard(int boardRow, int boardCol, int tileRow, int tileCol)
        {
            var board = GetBoardAtPosition(boardRow, boardCol);
            board.PlayTile(tileRow, tileCol, _currentPlayer);

            _nextBoardPos = new Tuple<int, int>(tileRow, tileCol);
            ChangeTurn();
        }

        public string GetSymbolForTileOnBoard(int boardRow, int boardCol, int tileRow, int tileCol)
        {
            return GetBoardAtPosition(boardRow, boardCol).GetSymbolForTile(tileRow, tileCol);  
        }

        protected override GameBoard GetEmptyTile()
        {
            return new GameBoard();
        }

        public bool GameOverOnBoard(int row, int col)
        {
            return Board[row][col].IsGameOver();
        }

        public Player GetWinnerOfBoard(int row, int col)
        {
            return Board[row][col].GetWinner();
        }
    }
}