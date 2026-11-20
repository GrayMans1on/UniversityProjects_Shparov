namespace MapEditor;

public class Map
{
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
            if (cell.Position.X > _width || cell.Position.Y > _width)
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
            if (cell.Position.X > _width || cell.Position.Y > _width)
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

    public Map()
    {
        
    }

    public bool AddCell(Cell cell)
    {
        if (cell.Position.X > Width || cell.Position.Y > Height) { return false; }
        _cells.Add(cell);
        return true;
    }
}
