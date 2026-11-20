namespace MapEditor;

public struct Cell
{
    private CellType _type;
    private Position _position;

    public Cell(CellType type, Position position)
    {
        _type = type;
        _position = position;
    }
    public CellType Type => _type;
    public Position Position => _position;

    public void ChangePosition(Position newPos)
    {
        _position = newPos;
    }
}
public enum CellType
{
    Empty,
    Wall,
    Water
}