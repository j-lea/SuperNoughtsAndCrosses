namespace SuperNoughtsAndCrosses.models
{
    public interface IGameState
    {
        void PlayTile(int row, int col);

        int GetNumberOfRows();

        int GetNumberOfCols();

        string GetSymbolForTile(int row, int col);
    }

    public enum PlayerSymbol
    {
        UNPLAYED,
        CROSS,
        NOUGHT
    }
}