using Microsoft.AspNetCore.Mvc;
using SuperNoughtsAndCrosses.models;

namespace SuperNoughtsAndCrosses.Controllers
{
    public class GameController : Controller
    {

        private readonly IGameState _gameState;
        
        public GameController(IGameState gameState)
        {
            _gameState = gameState;
        }

        public IActionResult Index()
        {
            return View(_gameState);
        }

        public IActionResult PlayTile(int row, int column)
        {
            try
            {
                _gameState.PlayTile(row, column);
            }
            catch (InvalidMoveException e)
            {
            }

            if (!_gameState.IsGameOver())
            {
                return PartialView("Board", _gameState);
            }

            object winner = _gameState.Display(_gameState.GetWinner());
            return PartialView("GameOverBoard", winner);
        }
    }
}