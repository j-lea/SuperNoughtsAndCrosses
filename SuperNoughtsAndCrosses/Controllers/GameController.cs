using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperNoughtsAndCrosses.models;

namespace SuperNoughtsAndCrosses.Controllers
{
    public class GameController : Controller
    {

        private readonly SuperGameBoard _gameBoard;
        private WebSocket _webSocket;
        
        public GameController(SuperGameBoard gameBoard)
        {
            _gameBoard = gameBoard;
        }

        public IActionResult Index()
        {
            return View(_gameBoard);
        }

        public IActionResult PlayTile(int tileRow, int tileCol, int boardRow, int boardCol)
        {
            if (!_gameBoard.IsGameOver())
            {
                try
                {
                    _gameBoard.PlayTileOnBoard(boardRow, boardCol, tileRow, tileCol);
                }
                catch
                {
                    // ignored
                }                
            }

            return PartialView("SuperBoard", _gameBoard);
        }

        public IActionResult RefreshBoard()
        {
            return PartialView("SuperBoard", _gameBoard);
        }

        private async void SendWebSocketMessage(object sender, EventArgs e)
        {
            byte[] bytes = Encoding.ASCII.GetBytes($"Woohoo websockets is working");

            if (_webSocket != null)
            {
                await _webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }

        }

        public async Task Ws()
        {
            var context = ControllerContext.HttpContext;

            if (context.WebSockets.IsWebSocketRequest)
            {
                _webSocket = await context.WebSockets.AcceptWebSocketAsync();
               // _gameBoard.TilePlayed += SendWebSocketMessage;
                for (var i = 0; i < 100; i++)
                    {
                        SendWebSocketMessage(null, null);
                        
                        Thread.Sleep(1000);
                    }
                //await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }
    }
}