using System;

namespace SuperNoughtsAndCrosses.models
{
    public interface IGameState
    {
        void PlayTile(int row, int col);

        int GetNumberOfRows();

        int GetNumberOfCols();

        string GetSymbolForTile(int row, int col);

        bool IsGameOver();

        Player GetWinner();

        string Display(Player player);
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