using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SuperNoughtsAndCrosses.models;

namespace SuperNoughtsAndCrosses.Hubs
{
    public class MoveHub : Hub
    {
        private SuperGameBoard _gameBoard;
        
        public MoveHub(SuperGameBoard gameBoard)
        {
            _gameBoard = gameBoard;
        }

        public async Task MakeMove(int tileRow, int tileCol, int boardRow, int boardCol)
        {
            if (!_gameBoard.IsGameOver())
            {
                try
                {
                    _gameBoard.PlayTileOnBoard(boardRow, boardCol, tileRow, tileCol);
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