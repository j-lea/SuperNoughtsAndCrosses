using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SuperNoughtsAndCrosses.models;

namespace SuperNoughtsAndCrosses.Hubs
{
    public class MoveHub : Hub
    {
        private GameManager _gameManager;
        
        public MoveHub(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public async Task MakeMove(int gameId, int tileRow, int tileCol, int boardRow, int boardCol)
        {
            var game = _gameManager.GetGame(gameId);
            if (!game.IsGameOver())
            {
                try
                {
                    game.PlayTileOnBoard(boardRow, boardCol, tileRow, tileCol);
                    await Clients.All.SendAsync("UpdateBoard");
                    
                }
                catch
                {
                    // ignored
                }                
            }
        }
    }
}