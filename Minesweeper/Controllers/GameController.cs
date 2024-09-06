using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Minesweeper.Logic;
using Minesweeper.Messages;

namespace Minesweeper.Controllers;

public class GameController : Controller
{
    private static readonly GameManager GameManager = new();
    
    [HttpPost]
    [Route("new")]
    public IActionResult StartNewGame([FromBody] NewGameRequest gameRequest)
    {
        Game newGame;
        try
        {
            newGame = GameManager.StartGame(gameRequest.width, gameRequest.height, gameRequest.mines_count);
        }
        catch (ArgumentException e)
        {
            return BadRequest(JsonSerializer.Serialize(new ErrorResponse() {error = e.Message}));
        }

        var gameState = new GameTurnRequest()
        {
            game_id = newGame.GameId,
            width = newGame.Width,
            height = newGame.Height,
            mines_count = newGame.MinesCount,
            field = newGame.Field,
            completed = newGame.IsGameCompleted
        };
        return Ok(JsonSerializer.Serialize(gameState));
    }
    
    [HttpPost]
    [Route("turn")]
    public IActionResult StartNewGame([FromBody] GameInfoResponse gameInfo)
    {
        var game = GameManager.GetGame(gameInfo.game_id);
        try
        {
            game.MakeTurn(gameInfo.row, gameInfo.col);
        }
        catch (ArgumentException e)
        {
            return BadRequest(JsonSerializer.Serialize(new ErrorResponse() {error = e.Message}));
        }
        
        var gameState = new GameTurnRequest()
        {
            game_id = game.GameId,
            width = game.Width,
            height = game.Height,
            mines_count = game.MinesCount,
            field = game.Field,
            completed = game.IsGameCompleted
        };
        return Ok(JsonSerializer.Serialize(gameState));
    }
}