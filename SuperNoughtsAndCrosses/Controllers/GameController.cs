using Microsoft.AspNetCore.Mvc;
using SuperNoughtsAndCrosses.models;

namespace SuperNoughtsAndCrosses.Controllers
{
    public class GameController : Controller
    {

        private readonly SuperGameBoard _gameBoard;
        
        public GameController(SuperGameBoard gameBoard)
        {
            _gameBoard = gameBoard;
        }

        public IActionResult Index()
        {
            return View(_gameBoard);
        }

        public IActionResult RefreshBoard()
        {
            return PartialView("SuperBoard", _gameBoard);
        }

    }
}