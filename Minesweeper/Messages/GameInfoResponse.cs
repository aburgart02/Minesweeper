namespace Minesweeper.Messages;

public class GameInfoResponse
{
    public string game_id { get; set; }
    public int col { get; set; }
    public int row { get; set; }
}