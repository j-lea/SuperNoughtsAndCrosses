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

        public void PlayTileOnBoard(int boardRow, int boardCol, int tileRow, int tileCol)
        {
            CheckThatBoardIsActive(boardRow, boardCol);
            
            CheckThatBoardIsValid(boardRow, boardCol);

            var board = GetBoardAtPosition(boardRow, boardCol);
            board.PlayTile(tileRow, tileCol, _currentPlayer);
            
            if (GameOverOnBoard(boardRow, boardCol))
            {
                var boardWinner = GetWinnerOfBoard(boardRow, boardCol);

                if (CheckForWin(boardRow, boardCol, boardWinner))
                {
                    _isGameOver = true;
                    Winner = boardWinner;
                    return;
                }
            }
            
            _nextBoardPos = new Tuple<int, int>(tileRow, tileCol);
            ChangeTurn();
        }
        
        private void CheckThatBoardIsActive(int row, int col)
        {
            if (GameOverOnBoard(row, col))
            {
                throw new BoardCompleteException();
            } 
        }

        private void CheckThatBoardIsValid(int row, int col)
        {
            if (_nextBoardPos == null) return;
            
            var (nextBoardRow, nextBoardCol) = _nextBoardPos;
            var incorrectBoard = row != nextBoardRow || col != nextBoardCol;
            if (incorrectBoard && !GameOverOnBoard(nextBoardRow, nextBoardCol))
            {
                throw new InvalidMoveException();
            }
        }

        protected override bool CheckPlayerWinsTile(GameBoard board, Player player)
        {
            return board.IsGameOver() && board.GetWinner().Equals(player);
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

        public bool IsNextBoard(int row, int col)
        {
            if (_nextBoardPos == null) return false;
            return _nextBoardPos.Item1 == row && _nextBoardPos.Item2 == col;
        }

        public bool GameOverOnNextBoard()
        {
            if (_nextBoardPos == null) return false;
            return GameOverOnBoard(_nextBoardPos.Item1, _nextBoardPos.Item2);
        }
    }
}