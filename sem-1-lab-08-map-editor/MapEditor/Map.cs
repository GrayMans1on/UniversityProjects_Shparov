namespace MapEditor;

public class Map
{
    private Player _player;

    public Player Player => _player;

    private List<Cell> _cells;
    public List<Cell> Cells => _cells;

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
            if (cell.Pos.X > _width || cell.Pos.Y > _height)
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
            if (cell.Pos.X > _width || cell.Pos.Y > _height)
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

    public Map(int width, int height, Cell[] cells, Player player) // п.1 создание карты
    {
        if (width <= 0 || height <= 0) { throw new Exception("Размеры карты должны быть положительными"); }
        _width = width;
        _height = height;
        _cells = cells.ToList();
        _player = player;
    }

    public bool AddCell(Cell cell)
    {
        if (cell.Pos.X > Width || cell.Pos.Y > Height) { return false; }
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
            cell.Pos.X == position.X && cell.Pos.Y == position.Y); // лямбда функция, возвращает номер в списке или -1 если нету
    }

    public bool TryMovePlayer(Direction dir) // п. перемещение игрока
    {
        Position newPos = _player.Pos + dir.ToPosition();

        if (newPos.X < 1 || newPos.X > Width || newPos.Y < 1 || newPos.Y > Height)
        {
            return false;
        }

        int index = FindCellIndex(newPos);

        if (index == -1 || !_player.BlockedCellTypes.Contains(_cells[index].Type))
        {
            _player.SetPosition(newPos);
            return true;
        }

        return false;
    }
    public override string ToString() // п.3 отображение карты в консоли
    {
        string result = "";

        for (int y = 1; y <= _height; y++)
        {
            for (int x = 1; x <= _width; x++)
            {
                if (_player.Pos == new Position(x, y ))
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

    public void ChangeCell(Cell newCell)  // п.2 изменение клеток
    {
        int index = FindCellIndex(newCell.Pos);
        if (index != -1)
        {
            RemoveCell(_cells[index]);
        }
        AddCell(newCell);
    }
}
