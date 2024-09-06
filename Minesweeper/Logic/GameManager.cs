namespace Minesweeper.Logic;

public class GameManager
{
    private Dictionary<string, Game> _games = new();

    public GameManager()
    {
        
    }

    public Game StartGame(int width, int height, int minesCount)
    {
        if (width < 2 || width > 30 || height < 2 || height > 30)
        {
            throw new ArgumentException("Ширина поля должна быть не менее 2 и не более 30");
        }
        if (minesCount < 1 || minesCount > width * height - 1)
        {
            throw new ArgumentException($"Количество мин должно быть не менее 1 и не более {width * height - 1}");
        }
        
        var gameState = new Game(width, height, minesCount);
        _games.Add(gameState.GameId, gameState);
        return gameState;
    }

    public Game GetGame(string gameId)
    {
        return _games[gameId];
    }
}