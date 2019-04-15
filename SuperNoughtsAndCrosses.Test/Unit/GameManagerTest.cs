using SuperNoughtsAndCrosses.models;
using Xunit;

namespace SuperNoughtsAndCrosses.Test.Unit
{
    public class GameManagerTest
    {
        [Fact]
        public void AddGameReturnsId()
        {
            var gameManager = new GameManager();

            var superGameBoard = new SuperGameBoard();
            var id = gameManager.Add(superGameBoard);
            Assert.Equal(1, id);
            
            var superGameBoard2 = new SuperGameBoard();
            var id2 = gameManager.Add(superGameBoard2);
            Assert.Equal(2, id2);

        } 
        
        [Fact]
        public void GetWithValidIdReturnsGame()
        {
            var gameManager = new GameManager();

            var superGameBoard = new SuperGameBoard();
            var id = gameManager.Add(superGameBoard);            
            Assert.Equal(superGameBoard, gameManager.GetGame(id));
            
            var superGameBoard2 = new SuperGameBoard();
            var id2 = gameManager.Add(superGameBoard2);
            Assert.Equal(superGameBoard2, gameManager.GetGame(id2));
        }
        
    }
}