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

    public void MovePlayer(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:

                break;
            case Direction.South:

                break;
            case Direction.East:

                break;
            case Direction.West:

                break;
            default:
                throw new Exception("Ошибка при задании направления");
        }
    }
}
