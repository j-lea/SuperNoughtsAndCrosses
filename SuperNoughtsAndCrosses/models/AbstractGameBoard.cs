using System;
using System.Linq;
using static System.Linq.Enumerable;
using static SuperNoughtsAndCrosses.models.Player;

namespace SuperNoughtsAndCrosses.models
{
    public abstract class AbstractGameBoard<T>
    {
        protected const int Size = 3;

        protected readonly T[][] Board;

        protected bool _isGameOver;
        protected Player Winner;
        
        protected AbstractGameBoard()
        {
            Board = GetEmptyBoard(); 

            _isGameOver = false;
            Winner = NONE;
        }
        
        public Player GetWinner()
        {
            return Winner;
        }
        
        public int GetSize()
        {
            return Size;
        }

        public bool IsGameOver()
        {
            return _isGameOver;
        }

        protected abstract T GetEmptyTile();

        private T[][] GetEmptyBoard()
        {
            return Range(0, Size)
                .Select(rows => Range(0, Size)
                    .Select(cols => GetEmptyTile())
                    .ToArray()).ToArray();
        }
        
        protected abstract bool CheckPlayerWinsTile(T tile, Player player);
        
        protected bool CheckForWin(int row, int col, Player player)
        {
            return CheckForWinConditionOnRow(row, player) || 
                   CheckForWinConditionOnCol(col, player) || 
                   CheckForWinConditionDiagonallyDown(player) ||
                   CheckForWinConditionDiagonallyUp(player);
        }

        private bool CheckForWinConditionOnRow(int row, Player player)
        {
            return Board[row].All(
                board => CheckPlayerWinsTile(board, player));
        }

        private bool CheckForWinConditionOnCol(int col, Player player)
        {
            return Board.All(r => CheckPlayerWinsTile(r[col], player));
        }

        private bool CheckForWinConditionDiagonallyDown(Player player)
        {
            return Range(0, Size).All(i => CheckPlayerWinsTile(Board[i][i], player)); 
        }

        private bool CheckForWinConditionDiagonallyUp(Player player)
        {
            var maxTileIndex = Size - 1;
            return Range(0, Size).All(i =>
                CheckPlayerWinsTile(Board[i][maxTileIndex - i], player));
        }

        protected string Display(Player player)
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
        
     
    }
    
    public enum Player
    {
        NONE,
        CROSS,
        NOUGHT
    }
  
    public class InvalidMoveException : Exception
    {
        
    }
}