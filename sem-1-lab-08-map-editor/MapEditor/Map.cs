namespace MapEditor;

public class Map
{
    private Player _player;

    public Position PlayerPosition => _player.Position;

    private List<Cell> _cells;

    private int _width;
    private int _height;

    public int Width => _width;
    public int Height => _height;

    public bool SetSize(int newWidth, int newHeight)
    {
        _width = newWidth;
        _height = newHeight;

        List<Cell> removedCells = new List<Cell>();
        foreach (Cell cell in _cells)
        {
            if (cell.Position.X > _width || cell.Position.Y > _height)
            {
                removedCells.Add(cell);
            }
        }

        foreach (Cell cell in removedCells)
        {
            _cells.Remove(cell);
        }


        return removedCells.Count > 0;
    }
    public bool SetSize(int newWidth, int newHeight, out List<Cell> removedCells)
    {
        _width = newWidth;
        _height = newHeight;

        removedCells = new List<Cell>(); // клетки, оказавшиеся за пределами карты удаляются, но выводятся через out
        foreach (Cell cell in _cells)
        {
            if (cell.Position.X > _width || cell.Position.Y > _height)
            {
                removedCells.Add(cell);
            }
        }

        foreach (Cell cell in removedCells)
        {
            _cells.Remove(cell);
        }


        return removedCells.Count > 0;
    }

    public Map(int width, int height, Cell[] cells, Player player)
    {
        if (width <= 0 || height <= 0) { throw new Exception("Размеры карты должны быть положительными"); }
        _width = width;
        _height = height;
        _cells = cells.ToList();
        _player = player;
    }

    public bool AddCell(Cell cell)
    {
        if (cell.Position.X > Width || cell.Position.Y > Height) { return false; }
        _cells.Add(cell);
        return true;
    }
    public void RemoveCell(Cell cell)
    {
        if (_cells.Contains(cell))
        {
            _cells.Remove(cell);
        }
    }
    public int FindCellIndex(Position position)
    {
        return _cells.FindIndex(cell =>
            cell.Position.X == position.X && cell.Position.Y == position.Y); // лямбда функция, возвращает номер в списке или -1 если нету
    }

    public bool TryMovePlayer(Direction dir)
    {
        Position offsetPosition = dir.ToPosition();

        int index = FindCellIndex(_player.Position + offsetPosition);

        if (index == -1 || !_player.BlockedCellTypes.Contains(_cells[index].Type))
        {
            _player.SetPosition(_player.Position + offsetPosition);
            return true;
        }
        return false;
    }
    public override string ToString()
    {
        string result = "";

        for (int y = 1; y <= _height; y++)
        {
            for (int x = 1; x <= _width; x++)
            {
                if (_player.Position == new Position(x, y ))
                {
                    result += "P";
                    continue;
                }

                int index = FindCellIndex(new Position(x, y));
                if (index == -1)
                {
                    result += ".";
                    continue;
                }


                Cell cell = _cells[index];

                result += cell.Type.ToSymbol();
            }
            result += "\n";
        }

        return result;
    }
}
