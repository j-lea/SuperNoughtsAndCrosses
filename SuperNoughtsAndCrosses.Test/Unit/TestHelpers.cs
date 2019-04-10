using SuperNoughtsAndCrosses.models;

namespace SuperNoughtsAndCrosses.Test.Unit
{
    public static class TestHelpers
    {
        public static bool IsEmptyGameState(GameBoard gameBoard)
        {
            for (var row = 0; row < gameBoard.GetSize(); row++)
            {
                for (var col = 0; col < gameBoard.GetSize(); col++)
                {
                    if (!gameBoard.GetSymbolForTile(row, col).Equals(""))
                    {
                        return false;
                    };
                }
            }

            return true;
        }
    }
}