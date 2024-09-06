namespace Minesweeper.Logic;

public class Game
{
    public readonly string GameId;
    public readonly int Width;
    public readonly int Height;
    public readonly int MinesCount;
    public string[][] Field;
    public bool IsGameCompleted;
    
    private int _turnNumber;
    private HashSet<(int, int)> _mines = new();

    public Game(int width, int height, int minesCount)
    {
        GameId = Guid.NewGuid().ToString();
        Width = width;
        Height = height;
        MinesCount = minesCount;
        Field = new string[Height][];
        InitializeField();
    }
    
    private void InitializeField()
    {
        for(var i = 0 ; i < Height; i ++)
        {
            Field[i] = new string[Width];
            for (var j = 0; j < Width; j++)
            {
                Field[i][j] = " ";
            }
        }
    }

    private void CheckTile(int row, int col)
    {
        if (IsGameCompleted)
        {
            throw new ArgumentException("Игра завершена");
        }
        if (Field[row][col] != " ")
        {
            throw new ArgumentException("Ячейка уже открыта");
        }
    }
    
    public void MakeTurn(int row, int col)
    {
        CheckTile(row, col);
        _turnNumber++;
        if (_turnNumber == 1)
        {
            GenerateMines(row, col);
            ProcessTurn(row, col);
        }
        else
        {
            if (_mines.Contains((row, col)))
            {
                OpenAllTiles(false);
            }
            else
            {
                ProcessTurn(row, col);
            }
        }

        var uncheckedTiles = GetUncheckedTiles().ToList();
        if (uncheckedTiles.Count == _mines.Count && uncheckedTiles.All(tile => _mines.Contains(tile)))
        {
            OpenAllTiles(true);
        }
    }

    private IEnumerable<(int, int)> GetUncheckedTiles()
    {
        var uncheckedTiles = new List<(int, int)>();
        for (var i = 0; i < Height; i++)
        {
            for (var j = 0; j < Width; j++)
            {
                if (Field[i][j] == " ")
                {
                    uncheckedTiles.Add((i, j));
                }
            }
        }

        return uncheckedTiles;
    }

    private void ProcessTurn(int row, int col)
    {
        var queue = new Queue<(int, int)>();
        queue.Enqueue((row, col));
        var visited = new HashSet<(int, int)>();
        while (queue.Count > 0)
        {
            var tile = queue.Dequeue();
            if (visited.Contains(tile)) continue;
            visited.Add(tile);
            var neighbours = GetNeighbours(tile.Item1, tile.Item2);
            var neighbourMinesCount = neighbours.Count(x => _mines.Contains(x));
            Field[tile.Item1][tile.Item2] = neighbourMinesCount.ToString();
            if (neighbourMinesCount != 0) continue;
            foreach (var neighbour in neighbours)
            {
                queue.Enqueue(neighbour);
            }
        }

    }

    private void GenerateMines(int row, int col)
    {
        var allTiles = new List<(int, int)>();
        
        for (var i = 0; i < Height; i++)
        {
            for (var j = 0; j < Width; j++)
            {
                if (!(i == row && j == col))
                {
                    allTiles.Add((i, j));
                }
            }
        }
        
        var rnd = new Random();
        _mines = allTiles.OrderBy(_ => rnd.Next()).Take(MinesCount).ToHashSet();
    }

    private List<(int, int)> GetNeighbours(int x, int y)
    {
        var neighbours = new List<(int, int)>();
        for (var i = -1; i < 2; i++)
        {
            for (var j = -1; j < 2; j++)
            {
                var row = x + i;
                var col = y + j;
                if (row < Height && row >= 0 && col < Width && col >= 0 && !(i == 0 && j ==0))
                {
                    neighbours.Add((row, col));
                }
            }
        }

        return neighbours;
    }

    private void OpenAllTiles(bool isGameWon)
    {
        IsGameCompleted = true;
        for (var i = 0; i < Height; i++)
        {
            for (var j = 0; j < Width; j++)
            {
                if (_mines.Contains((i, j)))
                {
                    Field[i][j] = isGameWon ? "M" : "X";
                }
                else
                {
                    Field[i][j] = GetNeighbours(i, j).Count(tile => _mines.Contains(tile)).ToString();  
                }
            }
        }
    }
}