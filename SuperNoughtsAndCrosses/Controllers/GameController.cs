using System;
using Microsoft.AspNetCore.Mvc;
using SuperNoughtsAndCrosses.models;

namespace SuperNoughtsAndCrosses.Controllers
{
    public class GameController : Controller
    {

        private readonly GameManager _gameManager;
        
        public GameController(GameManager gameManager)
        {
            _gameManager = gameManager;
        }
        
        public IActionResult NewGame()
        {
            var game = new SuperGameBoard();
            var id = _gameManager.Add(game);
            return Redirect($"{id}");
        }

        public IActionResult Index(int gameId)
        {
            var gameBoard = _gameManager.GetGame(gameId);
            return View(gameBoard);
        }

        public IActionResult RefreshBoard(int gameId)
        {
            return PartialView("SuperBoard", _gameManager.GetGame(gameId));
        }

    }
}