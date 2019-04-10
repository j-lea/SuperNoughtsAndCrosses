using SuperNoughtsAndCrosses.models;
using Xunit;

namespace SuperNoughtsAndCrosses.Test.Unit
{
    public class GameBoardTest
    {
        private readonly GameBoard _gameBoard;

        public GameBoardTest()
        {
            _gameBoard = new GameBoard();
        }

        [Fact]
        public void GameBoardHasCorrectDimensions()
        {
            Assert.Equal(3, _gameBoard.GetSize());
        }

        [Fact]
        public void BoardIsEmptyWhenTheGameStarts()
        {
            Assert.True(TestHelpers.IsEmptyGameState(_gameBoard));
        }

        [Fact]
        public void PlayerCrossShowsAnX()
        {
            const int row = 0;
            const int col = 1;
            
            _gameBoard.PlayTile(row, col, Player.CROSS);
            
            Assert.Equal("X", _gameBoard.GetSymbolForTile(row, col));
        }

        [Fact]
        public void PlayerNoughtShowsAnO()
        {
            const int row = 0;
            const int col = 1;
            
            _gameBoard.PlayTile(row, col, Player.NOUGHT);
            
            Assert.Equal("O", _gameBoard.GetSymbolForTile(row, col));
        }

        [Fact]
        public void ThrowExceptionWhenTileIsPlayedTwice()
        {
            const int row = 0;
            const int col = 1;
            
            _gameBoard.PlayTile(row, col, Player.CROSS);
            
            Assert.Throws<InvalidMoveException>(() => 
                _gameBoard.PlayTile(row, col, Player.NOUGHT));
            Assert.Equal("X", _gameBoard.GetSymbolForTile(row, col));
        }

        [Fact]
        public void GameIsOverWithATieWhenAllTilesHaveBeenPlayed()
        {
            Assert.False(_gameBoard.IsGameOver());

            _gameBoard.PlayTile(2, 2, Player.CROSS);
            _gameBoard.PlayTile(0, 0, Player.NOUGHT);
            _gameBoard.PlayTile(0, 1, Player.CROSS);
            _gameBoard.PlayTile(0, 2, Player.NOUGHT);
            _gameBoard.PlayTile(1, 0, Player.CROSS);
            _gameBoard.PlayTile(1, 1, Player.NOUGHT);
            _gameBoard.PlayTile(1, 2, Player.CROSS);
            _gameBoard.PlayTile(2, 1, Player.NOUGHT);
            _gameBoard.PlayTile(2, 0, Player.CROSS);
            
            Assert.True(_gameBoard.IsGameOver());
            Assert.Equal(Player.NONE, _gameBoard.GetWinner());
        }

        [Fact]
        public void GameIsOverWhenCrossesHasWonHorizontally()
        {
            _gameBoard.PlayTile(0, 0, Player.CROSS);
            _gameBoard.PlayTile(1, 0, Player.NOUGHT);
            _gameBoard.PlayTile(0, 1, Player.CROSS);
            _gameBoard.PlayTile(2, 0, Player.NOUGHT);
            _gameBoard.PlayTile(0, 2, Player.CROSS);
            
            Assert.True(_gameBoard.IsGameOver());
            Assert.Equal(Player.CROSS, _gameBoard.GetWinner());
        }
        
        [Fact]
        public void GameIsOverWhenNoughtsHasWonVertically()
        {
            _gameBoard.PlayTile(2, 2, Player.CROSS);
            _gameBoard.PlayTile(0, 0, Player.NOUGHT);
            _gameBoard.PlayTile(1, 1, Player.CROSS);
            _gameBoard.PlayTile(1, 0, Player.NOUGHT);
            _gameBoard.PlayTile(2, 1, Player.CROSS);
            _gameBoard.PlayTile(2, 0, Player.NOUGHT);
            
            Assert.True(_gameBoard.IsGameOver());
            Assert.Equal(Player.NOUGHT, _gameBoard.GetWinner());
        }

        [Fact]
        public void GameIsOverWhenCrossesHasWonDiagonallyDown()
        {
            _gameBoard.PlayTile(0, 0, Player.CROSS);
            _gameBoard.PlayTile(1, 0, Player.NOUGHT);
            _gameBoard.PlayTile(1, 1, Player.CROSS);
            _gameBoard.PlayTile(2, 1, Player.NOUGHT);
            _gameBoard.PlayTile(2, 2, Player.CROSS);
            
            Assert.True(_gameBoard.IsGameOver());
            Assert.Equal(Player.CROSS, _gameBoard.GetWinner());
        }
        
        [Fact]
        public void GameIsOverWhenCrossesHasWonDiagonallyUp()
        {
            _gameBoard.PlayTile(2, 0, Player.CROSS);
            _gameBoard.PlayTile(1, 0, Player.NOUGHT);
            _gameBoard.PlayTile(1, 1, Player.CROSS);
            _gameBoard.PlayTile(1, 2, Player.NOUGHT);
            _gameBoard.PlayTile(0, 2, Player.CROSS);
            
            Assert.True(_gameBoard.IsGameOver());
            Assert.Equal(Player.CROSS, _gameBoard.GetWinner());
        }
    }
}