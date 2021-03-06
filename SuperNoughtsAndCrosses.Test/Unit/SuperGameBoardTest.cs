using SuperNoughtsAndCrosses.models;
using Xunit;
using static System.Linq.Enumerable;

namespace SuperNoughtsAndCrosses.Test.Unit
{
    public class SuperGameBoardTest
    {
        private readonly SuperGameBoard _superGameBoard;

        public SuperGameBoardTest()
        {
            _superGameBoard = new SuperGameBoard();
        }

        [Fact]
        public void SuperGameBoardHasCorrectDimensions()
        {
            Assert.Equal(3, _superGameBoard.GetSize());
        }

        [Fact]
        public void AllBoardsAreEmptyWhenTheGameStarts()
        {
            for (var row = 0; row < _superGameBoard.GetSize(); row++)
            {
                for (var col = 0; col < _superGameBoard.GetSize(); col++)
                {
                    GameBoard gameBoard = _superGameBoard.GetBoardAtPosition(row, col);
                    Assert.True(TestHelpers.IsEmptyGameState(gameBoard));
                }
            }
        }

        [Fact]
        public void FirstMoveIsCrossOnlyOnSelectedBoard()
        {
            _superGameBoard.PlayTileOnBoard(1, 0, 2, 1);
            
            Assert.Equal("X", _superGameBoard.GetSymbolForTileOnBoard(1, 0, 2, 1));
            Assert.True(EveryBoardIsEmptyExcept(1, 0));
        }

        [Fact]
        public void SecondMoveSucceedsInCorrespondingBoard()
        {
            var firstMoveTilePosition = (Row: 2, Col: 1);
            
            _superGameBoard.PlayTileOnBoard(
                1, 0, firstMoveTilePosition.Row, firstMoveTilePosition.Col);
            
            _superGameBoard.PlayTileOnBoard(firstMoveTilePosition.Row, firstMoveTilePosition.Col, 0, 1);
            
            Assert.Equal("O", _superGameBoard.GetSymbolForTileOnBoard(
                firstMoveTilePosition.Row, firstMoveTilePosition.Col, 0, 1));
        }

        [Fact]
        public void KnowsWhenOneBoardIsComplete()
        {
            _superGameBoard.PlayTileOnBoard(0, 0, 1, 0);
            _superGameBoard.PlayTileOnBoard(1, 0, 0, 0);
            _superGameBoard.PlayTileOnBoard(0, 0, 1, 1);
            _superGameBoard.PlayTileOnBoard(1, 1, 0, 0);
            _superGameBoard.PlayTileOnBoard(0, 0, 1, 2);

            Assert.True(_superGameBoard.GameOverOnBoard(0, 0));
            Assert.Equal(Player.CROSS, _superGameBoard.GetWinnerOfBoard(0, 0));
        }
        
        [Fact]
        public void ThrowsExceptionWhenTileIsPlayedOnACompleteBoard()
        {
            _superGameBoard.PlayTileOnBoard(0, 0, 1, 0);
            _superGameBoard.PlayTileOnBoard(1, 0, 0, 0);
            _superGameBoard.PlayTileOnBoard(0, 0, 1, 1);
            _superGameBoard.PlayTileOnBoard(1, 1, 0, 0);
            _superGameBoard.PlayTileOnBoard(0, 0, 1, 2);
            _superGameBoard.PlayTileOnBoard(1, 2, 0, 0);

            Assert.Throws<BoardCompleteException>(() => _superGameBoard.PlayTileOnBoard(0, 0,2, 2));
        }
        
        [Fact]
        public void PlayerCanPlayOnAnyBoardIfDirectedToCompleteBoard()
        {
            _superGameBoard.PlayTileOnBoard(0, 0, 1, 0);
            _superGameBoard.PlayTileOnBoard(1, 0, 0, 0);
            _superGameBoard.PlayTileOnBoard(0, 0, 1, 1);
            _superGameBoard.PlayTileOnBoard(1, 1, 0, 0);
            _superGameBoard.PlayTileOnBoard(0, 0, 1, 2);
            _superGameBoard.PlayTileOnBoard(1, 2, 0, 0);
            
            _superGameBoard.PlayTileOnBoard(2, 2, 2, 2);
            Assert.Equal("X", _superGameBoard.GetSymbolForTileOnBoard(2, 2, 2, 2));
        }

        [Fact]
        public void ThrowsExceptionWhenPlayerTriesToPlayOnBoardTheyHaveNotBeenDirectedTo()
        {
            _superGameBoard.PlayTileOnBoard(0, 0, 1, 0);
            Assert.Throws<InvalidMoveException>(() => _superGameBoard.PlayTileOnBoard(0, 0, 2, 0));
        }

        [Fact]
        public void KnowsWhenSuperBoardIsCompleteAndCrossWins()
        {
            _superGameBoard.PlayTileOnBoard(0, 0, 1, 0);
            _superGameBoard.PlayTileOnBoard(1, 0, 0, 0);
            _superGameBoard.PlayTileOnBoard(0, 0, 1, 1);
            _superGameBoard.PlayTileOnBoard(1, 1, 0, 0);
            _superGameBoard.PlayTileOnBoard(0, 0, 1, 2);
            Assert.Equal(Player.CROSS, _superGameBoard.GetWinnerOfBoard(0, 0));
            
            _superGameBoard.PlayTileOnBoard(1, 2, 0, 1);
            _superGameBoard.PlayTileOnBoard(0, 1, 1, 0);
            _superGameBoard.PlayTileOnBoard(1, 0, 0, 1);
            _superGameBoard.PlayTileOnBoard(0, 1, 1, 1);
            _superGameBoard.PlayTileOnBoard(1, 1, 0, 1);
            _superGameBoard.PlayTileOnBoard(0, 1, 1, 2);
            Assert.Equal(Player.CROSS, _superGameBoard.GetWinnerOfBoard(0, 1));
            
            _superGameBoard.PlayTileOnBoard(1, 2, 0, 2);
            _superGameBoard.PlayTileOnBoard(0, 2, 2, 0);
            _superGameBoard.PlayTileOnBoard(2, 0, 0, 2);
            _superGameBoard.PlayTileOnBoard(0, 2, 2, 1);
            _superGameBoard.PlayTileOnBoard(2, 1, 0, 2);
            _superGameBoard.PlayTileOnBoard(0, 2, 2, 2);
            Assert.Equal(Player.CROSS, _superGameBoard.GetWinnerOfBoard(0, 2));
            
            Assert.True(_superGameBoard.IsGameOver());
            Assert.Equal(Player.CROSS, _superGameBoard.GetWinner());
        }
        
        [Fact]
        public void KnowsWhenSuperBoardIsCompleteAndNoughtWins()
        {
            _superGameBoard.PlayTileOnBoard(0, 0, 0, 0);
            _superGameBoard.PlayTileOnBoard(0, 0, 1, 0);
            _superGameBoard.PlayTileOnBoard(1, 0, 0, 0);
            _superGameBoard.PlayTileOnBoard(0, 0, 1, 1);
            _superGameBoard.PlayTileOnBoard(1, 1, 0, 0);
            _superGameBoard.PlayTileOnBoard(0, 0, 1, 2);
            Assert.Equal(Player.NOUGHT, _superGameBoard.GetWinnerOfBoard(0, 0));
            
            _superGameBoard.PlayTileOnBoard(1, 2, 0, 1);
            _superGameBoard.PlayTileOnBoard(0, 1, 1, 0);
            _superGameBoard.PlayTileOnBoard(1, 0, 0, 1);
            _superGameBoard.PlayTileOnBoard(0, 1, 1, 1);
            _superGameBoard.PlayTileOnBoard(1, 1, 0, 1);
            _superGameBoard.PlayTileOnBoard(0, 1, 1, 2);
            Assert.Equal(Player.NOUGHT, _superGameBoard.GetWinnerOfBoard(0, 1));
            
            _superGameBoard.PlayTileOnBoard(1, 2, 0, 2);
            _superGameBoard.PlayTileOnBoard(0, 2, 2, 0);
            _superGameBoard.PlayTileOnBoard(2, 0, 0, 2);
            _superGameBoard.PlayTileOnBoard(0, 2, 2, 1);
            _superGameBoard.PlayTileOnBoard(2, 1, 0, 2);
            _superGameBoard.PlayTileOnBoard(0, 2, 2, 2);
            Assert.Equal(Player.NOUGHT, _superGameBoard.GetWinnerOfBoard(0, 2));
            
            Assert.True(_superGameBoard.IsGameOver());
            Assert.Equal(Player.NOUGHT, _superGameBoard.GetWinner());
        }

        private bool EveryBoardIsEmptyExcept(int boardRow, int boardCol)
        {
            return Range(0, _superGameBoard.GetSize())
                .SelectMany(br => Range(0, _superGameBoard.GetSize())
                    .Select(bc => (Row: br, Col: bc)))
                .Where(boardPos => !(boardPos).Equals((boardRow, boardCol)))
                .All(boardPos => TestHelpers.IsEmptyGameState(
                    _superGameBoard.GetBoardAtPosition(boardPos.Row, boardPos.Col)));
        }
    }
}