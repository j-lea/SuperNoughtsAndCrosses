using System;

namespace SuperNoughtsAndCrosses.models
{
    public interface IGameState
    {
        void PlayTile(int row, int col);

        int GetNumberOfRows();

        int GetNumberOfCols();

        string GetSymbolForTile(int row, int col);
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